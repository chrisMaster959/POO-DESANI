using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

public class ClienteController : Controller
{
  public ActionResult Agenda()
  {
    return View();
  }
  public ActionResult Barbeiro()
  {
    return View();
  }

  public ActionResult Agendamento(int id)
  {
    var servicos = new List<Servico>
    {
        new Servico(1, "Corte de cabelo", 30.0),
        new Servico(2, "Barba", 20.0),
        new Servico(3, "Sobrancelha", 15.0),
        new Servico(4, "Corte + Barba", 45.0),
        new Servico(5, "Corte + Sobrancelha", 40.0)
    };

    var servico = servicos.FirstOrDefault(s => s.Id == id);
    if (servico == null) return NotFound();

    var barbeiros = new List<Barbeiro>
    {
        new Barbeiro { Id = 1, Nome = "João", Logradouro = "Rua A", Nr_Logradouro = 10, Bairro = "Centro", cep = 12345, ServicosIds = new List<int> {1,2,4} },
        new Barbeiro { Id = 2, Nome = "Maria", Logradouro = "Rua B", Nr_Logradouro = 20, Bairro = "Bairro X", cep = 67890, ServicosIds = new List<int> {3,5} },
        new Barbeiro { Id = 3, Nome = "Pedro", Logradouro = "Rua C", Nr_Logradouro = 30, Bairro = "Bairro Y", cep = 54321, ServicosIds = new List<int> {1,3,4,5} }
    };

    var barbeirosDisponiveis = barbeiros.Where(b => b.ServicosIds.Contains(id)).ToList();

    ViewBag.ServicoNome = servico.Nome;
    return View(barbeirosDisponiveis);
  }
}