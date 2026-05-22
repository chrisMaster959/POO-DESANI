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
    ViewBag.Ceps = db.Cep.ToList();

    return View();
  }

[HttpPost]
public IActionResult Cadastro(Barbeiro barbeiroForm)
{
    var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

    if (usuarioId == null)
    {
        return RedirectToAction("Login", "Pessoa");
    }

    // verifica se já é barbeiro
    var barbeiroExiste = db.Barbeiro
        .Any(b => b.Id == usuarioId);

    if (barbeiroExiste)
{
    var barbeiro = db.Barbeiro
        .Include(b => b.Servicos)
        .FirstOrDefault(b => b.Id == usuarioId);

    if(barbeiro.Servicos == null || !barbeiro.Servicos.Any())
    {
        return RedirectToAction("Escolha", "Servico");
    }

    return RedirectToAction("Controle");
    }

    // verifica se pessoa existe
    var pessoaExiste = db.Pessoa
        .Any(p => p.Id == usuarioId);

    if (!pessoaExiste)
    {
        return NotFound();
    }

    // INSERT SOMENTE na tabela derivada
    db.Database.ExecuteSqlRaw(@"
        INSERT INTO Barbeiro
        (
            Id,
            Logradouro,
            Nr_Logradouro,
            Bairro,
            CepId
        )
        VALUES
        (
            {0},
            {1},
            {2},
            {3},
            {4}
        )
    ",
    usuarioId,
    barbeiroForm.Logradouro,
    barbeiroForm.Nr_Logradouro,
    barbeiroForm.Bairro,
    barbeiroForm.CepId);

    return RedirectToAction("Escolha", "Servico");
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