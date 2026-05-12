using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class BarbeiroController : Controller
{
  private readonly DatabaseContext db;

  public BarbeiroController(DatabaseContext db)
  {
    this.db = db;
  }

  [HttpGet]
  public ActionResult Cadastro()
  {
    return View();
  }

[HttpPost]
public ActionResult Cadastro(Barbeiro barbeiroForm)
{
    var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

    if(usuarioId == null)
    {
        return RedirectToAction("Login", "Pessoa");
    }

    // verifica se já é barbeiro
    var barbeiroExiste = db.Barbeiro
        .FirstOrDefault(b => b.Id == usuarioId);

    if(barbeiroExiste != null)
    {
        return RedirectToAction("Controle");
    }

    // busca os dados da pessoa
    var pessoa = db.Pessoa
        .FirstOrDefault(p => p.Id == usuarioId);

    if(pessoa == null)
    {
        return NotFound();
    }

    // cria o barbeiro usando o MESMO ID da pessoa
    var barbeiro = new Barbeiro
    {
        Id = pessoa.Id,
        Nome = pessoa.Nome,
        Email = pessoa.Email,
        Senha = pessoa.Senha,
        Telefone = pessoa.Telefone,

        Logradouro = barbeiroForm.Logradouro,
        Nr_Logradouro = barbeiroForm.Nr_Logradouro,
        Bairro = barbeiroForm.Bairro,
        CepId = barbeiroForm.CepId
    };

    db.Barbeiro.Add(barbeiro);

    db.SaveChanges();

    return RedirectToAction("Controle");
}

  public ActionResult Controle()
{
    var barbeiroId = HttpContext.Session.GetInt32("UsuarioId");

    if (barbeiroId == null)
    {
        return RedirectToAction("Login", "Pessoa");
    }

    var hoje = DateTime.Today;

    // Busca o barbeiro logado
    var barbeiro = db.Barbeiro
        .Include(b => b.Atendimentos)
            .ThenInclude(a => a.Cliente)
        .Include(b => b.Atendimentos)
            .ThenInclude(a => a.Servicos)
        .FirstOrDefault(b => b.Id == barbeiroId);

    if (barbeiro == null)
    {
        return NotFound();
    }

    // Geração dos horários
    var horarios = new List<DateTime>();

    // manhã
    var inicioManha = hoje.AddHours(8);
    var fimManha = hoje.AddHours(12);

    // tarde
    var inicioTarde = hoje.AddHours(13);
    var fimTarde = hoje.AddHours(19);

    for (var h = inicioManha; h < fimManha; h = h.AddMinutes(40))
    {
        horarios.Add(h);
    }

    for (var h = inicioTarde; h < fimTarde; h = h.AddMinutes(40))
    {
        horarios.Add(h);
    }

    ViewBag.Horarios = horarios;

    return View(barbeiro);
}

  public ActionResult Relatorio()
  {
    return View();
  }
}