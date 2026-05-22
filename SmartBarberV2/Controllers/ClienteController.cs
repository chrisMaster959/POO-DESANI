using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;

public class ClienteController : Controller
{
  private readonly DatabaseContext db;

  public ClienteController(DatabaseContext db)
  {
    this.db = db;
  }

    public ActionResult BarbeirosDisponiveis(int id)
{
    var servico = db.Servico.FirstOrDefault(s => s.Id == id);

    if (servico == null)
        return NotFound();

    // CORREÇÃO: filtra via tabela de junção BarbeiroServico diretamente,
    // evitando o erro de tradução LINQ com coleções de navegação
    var barbeirosIds = db.Set<Dictionary<string, object>>("BarbeiroServico")
        .Where(bs => (int)bs["ServicosId"] == id)
        .Select(bs => (int)bs["BarbeirosId"])
        .ToList();

    var barbeirosDisponiveis = db.Barbeiro
        .Where(b => barbeirosIds.Contains(b.Id))
        .ToList();

    ViewBag.ServicoNome = servico.Nome;

    return View("BarbeirosDisponiveis", barbeirosDisponiveis);
}

  public ActionResult Agendar(int barbeiroId)
  {
    var hoje = DateTime.Today;

    var horarios = new List<DateTime>();

    // manhã
    var inicioManha = hoje.AddHours(8);
    var fimManha = hoje.AddHours(12);

    // tarde
    var inicioTarde = hoje.AddHours(13);
    var fimTarde = hoje.AddHours(19);

    // gera manhã
    for(var h = inicioManha; h < fimManha; h = h.AddMinutes(40))
    {
        horarios.Add(h);
    }

    // gera tarde
    for(var h = inicioTarde; h < fimTarde; h = h.AddMinutes(40))
    {
        horarios.Add(h);
    }

    // pega atendimentos já marcados
    var agendados = db.Atendimento
        .Where(a =>
          a.BarbeiroId == barbeiroId &&
          a.DataHora >= hoje &&
          a.DataHora < hoje.AddDays(1))
        .Select(a => a.DataHora)
        .ToList();

    // remove horários ocupados
    horarios = horarios
        .Where(h => !agendados.Contains(h))
        .ToList();

    return View(horarios);
  }
  [HttpPost]
public IActionResult Agendar(int barbeiroId, DateTime horario)
{
    var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

    if (usuarioId == null)
    {
        return RedirectToAction("Login", "Pessoa");
    }

    var atendimento = new Atendimento
    {
        ClienteId = usuarioId.Value,
        BarbeiroId = barbeiroId,
        DataHora = horario,
        Status = "Agendado"
    };

    db.Atendimento.Add(atendimento);

    db.SaveChanges();

    return RedirectToAction("Login", "Pessoa");
}
}