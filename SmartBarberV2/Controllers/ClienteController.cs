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

    // LISTA BARBEIROS DISPONÍVEIS PARA O SERVIÇO
    public ActionResult BarbeirosDisponiveis(int id)
    {
        var servico = db.Servico.FirstOrDefault(s => s.Id == id);

        if (servico == null)
            return NotFound();

        // Busca barbeiros vinculados ao serviço
        var barbeirosIds = db.Set<Dictionary<string, object>>("BarbeiroServico")
            .Where(bs => (int)bs["ServicosId"] == id)
            .Select(bs => (int)bs["BarbeirosId"])
            .ToList();

        var barbeirosDisponiveis = db.Barbeiro
            .Where(b => barbeirosIds.Contains(b.Id))
            .ToList();

        ViewBag.ServicoNome = servico.Nome;
        ViewBag.ServicoId = id;

        return View("BarbeirosDisponiveis", barbeirosDisponiveis);
    }

    // TELA DE HORÁRIOS DISPONÍVEIS
    public ActionResult Agendar(int barbeiroId, int servicoId, DateTime? dia)
    {
        // Usa o dia selecionado ou hoje
        var diaSelecionado = dia?.Date ?? DateTime.Today;

        var horarios = new List<DateTime>();

        // HORÁRIOS DA MANHÃ
        var inicioManha = diaSelecionado.AddHours(8);
        var fimManha = diaSelecionado.AddHours(12);

        // HORÁRIOS DA TARDE
        var inicioTarde = diaSelecionado.AddHours(13);
        var fimTarde = diaSelecionado.AddHours(19);

        // Gera horários da manhã
        for (var h = inicioManha; h < fimManha; h = h.AddMinutes(40))
        {
            horarios.Add(h);
        }

        // Gera horários da tarde
        for (var h = inicioTarde; h < fimTarde; h = h.AddMinutes(40))
        {
            horarios.Add(h);
        }

        // Busca horários já agendados
        var agendados = db.Atendimento
            .Where(a =>
                a.BarbeiroId == barbeiroId &&
                a.DataHora >= diaSelecionado &&
                a.DataHora < diaSelecionado.AddDays(1))
            .Select(a => a.DataHora)
            .ToList();

        // Hora atual
        var agora = DateTime.Now;

        // Remove:
        // 1. horários ocupados
        // 2. horários passados do dia atual
        // 3. horários com menos de 30 min de antecedência
        horarios = horarios
            .Where(h =>
                !agendados.Contains(h) &&
                (
                    diaSelecionado.Date > agora.Date ||
                    h > agora.AddMinutes(30)
                )
            )
            .ToList();

        ViewBag.BarbeiroId = barbeiroId;
        ViewBag.ServicoId = servicoId;
        ViewBag.DiaSelecionado = diaSelecionado.ToString("yyyy-MM-dd");

        return View(horarios);
    }

    // CONFIRMAR AGENDAMENTO
    [HttpPost]
    public IActionResult Agendar(int barbeiroId, DateTime horario, int servicoId)
    {
        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

        if (usuarioId == null)
            return RedirectToAction("Login", "Pessoa");

        // Verifica se já existe atendimento nesse horário
        var horarioOcupado = db.Atendimento.Any(a =>
            a.BarbeiroId == barbeiroId &&
            a.DataHora == horario);

        if (horarioOcupado)
        {
            TempData["Erro"] = "Esse horário já foi agendado.";
            return RedirectToAction("Agendar", new
            {
                barbeiroId = barbeiroId,
                servicoId = servicoId,
                dia = horario.Date.ToString("yyyy-MM-dd")
            });
        }

        // Impede agendamento no passado
        if (horario <= DateTime.Now.AddMinutes(30))
        {
            TempData["Erro"] = "Horário inválido.";
            return RedirectToAction("Agendar", new
            {
                barbeiroId = barbeiroId,
                servicoId = servicoId,
                dia = horario.Date.ToString("yyyy-MM-dd")
            });
        }

        // Cria atendimento
        var atendimento = new Atendimento
        {
            ClienteId = usuarioId.Value,
            BarbeiroId = barbeiroId,
            DataHora = horario,
            Status = "Agendado"
        };

        // Adiciona serviço
        if (servicoId > 0)
        {
            var servico = db.Servico.FirstOrDefault(s => s.Id == servicoId);

            if (servico != null)
            {
                atendimento.Servicos.Add(servico);
            }
        }

        db.Atendimento.Add(atendimento);
        db.SaveChanges();

        return RedirectToAction("Atendimentos", "Atendimento");
    }
}