using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartBarberV2.Filters;

[Autenticacao]
public class ServicoController : Controller
{
    private readonly DatabaseContext db;

    public ServicoController(DatabaseContext db)
    {
        this.db = db;
    }

    // tela que mostra todos os serviços
    public ActionResult Servicos()
    {
        var servicos = db.Servico.ToList();

        return View(servicos);
    }

    // GET
    // abre a tela Escolha.cshtml
    public ActionResult Escolha()
{
    var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

    var servicos = db.Servico.ToList();

    // Busca os ids dos serviços que o barbeiro já possui
    if (usuarioId != null)
    {
        var barbeiro = db.Barbeiro
            .Include(b => b.Servicos)
            .FirstOrDefault(b => b.Id == usuarioId);

        var servicosDoBarbreiroIds = barbeiro?.Servicos?.Select(s => s.Id).ToList() ?? new List<int>();

        ViewBag.ServicosDoBarbreiroIds = servicosDoBarbreiroIds;
    }
    else
    {
        ViewBag.ServicosDoBarbreiroIds = new List<int>();
    }

    return View(servicos);
}

    // POST
    [HttpPost]
    public ActionResult SalvarServicosBarbeiro(List<int> servicosIds)
    {
        if(servicosIds == null || !servicosIds.Any())
        {
            return View("Escolha", db.Servico.ToList());
        }
        // pega barbeiro logado
        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

        if(usuarioId == null)
        {
            return RedirectToAction("Login", "Pessoa");
        }

        // busca barbeiro no banco
        var barbeiro = db.Barbeiro
            .Include(b => b.Servicos)
            .FirstOrDefault(b => b.Id == usuarioId);

        if(barbeiro == null)
        {
            return NotFound();
        }
        if(barbeiro.Servicos == null)
        {
        barbeiro.Servicos = new List<Servico>();
        }
        // limpa os serviços antigos
        barbeiro.Servicos.Clear();

        // busca os serviços selecionados
        var servicosSelecionados = db.Servico
            .Where(s => servicosIds.Contains(s.Id))
            .ToList();

        // adiciona os serviços ao barbeiro
        foreach(var servico in servicosSelecionados)
        {
            barbeiro.Servicos.Add(servico);
        }

        db.SaveChanges();

        return RedirectToAction("Controle", "Barbeiro");
    }
}