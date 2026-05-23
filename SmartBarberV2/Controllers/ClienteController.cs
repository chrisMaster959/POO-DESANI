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
        ViewBag.ServicoId = id; // CORREÇÃO: passa o id do serviço para a view

        return View("BarbeirosDisponiveis", barbeirosDisponiveis);
    }

    public ActionResult Agendar(int barbeiroId, int servicoId, DateTime? dia)
    {
        var diaSelecionado = dia?.Date ?? DateTime.Today;

        var horarios = new List<DateTime>();

        var inicioManha = diaSelecionado.AddHours(8);
        var fimManha = diaSelecionado.AddHours(12);
        var inicioTarde = diaSelecionado.AddHours(13);
        var fimTarde = diaSelecionado.AddHours(19);

        for (var h = inicioManha; h < fimManha; h = h.AddMinutes(40))
            horarios.Add(h);

        for (var h = inicioTarde; h < fimTarde; h = h.AddMinutes(40))
            horarios.Add(h);

        var agendados = db.Atendimento
            .Where(a =>
                a.BarbeiroId == barbeiroId &&
                a.DataHora >= diaSelecionado &&
                a.DataHora < diaSelecionado.AddDays(1))
            .Select(a => a.DataHora)
            .ToList();

        horarios = horarios
            .Where(h => !agendados.Contains(h))
            .ToList();

        ViewBag.BarbeiroId = barbeiroId;
        ViewBag.ServicoId = servicoId;
        ViewBag.DiaSelecionado = diaSelecionado.ToString("yyyy-MM-dd");

        return View(horarios);
    }

    [HttpPost]
    public IActionResult Agendar(int barbeiroId, DateTime horario, int servicoId)
    {
        System.Console.WriteLine($">>> barbeiroId: {barbeiroId}, servicoId: {servicoId}, horario: {horario}");

        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

        if (usuarioId == null)
            return RedirectToAction("Login", "Pessoa");

        var atendimento = new Atendimento
        {
            ClienteId = usuarioId.Value,
            BarbeiroId = barbeiroId,
            DataHora = horario,
            Status = "Agendado"
        };

        if (servicoId > 0)
        {
            var servico = db.Servico.FirstOrDefault(s => s.Id == servicoId);
            if (servico != null)
                atendimento.Servicos.Add(servico);
        }

        db.Atendimento.Add(atendimento);
        db.SaveChanges();

        return RedirectToAction("Atendimentos", "Atendimento");
    }
}