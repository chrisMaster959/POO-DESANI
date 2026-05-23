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

public ActionResult Controle(DateTime? dia)
{
    var barbeiroId = HttpContext.Session.GetInt32("UsuarioId");

    if (barbeiroId == null)
        return RedirectToAction("Login", "Pessoa");

    var diaSelecionado = dia?.Date ?? DateTime.Today;

    var barbeiro = db.Barbeiro
        .Include(b => b.Atendimentos)
            .ThenInclude(a => a.Servicos)
        .FirstOrDefault(b => b.Id == barbeiroId);

    if (barbeiro == null)
        return NotFound();

    // CORREÇÃO: busca clientes via SQL raw para evitar o bug de cast do TPT
    foreach (var atendimento in barbeiro.Atendimentos)
{
    var conn = db.Database.GetDbConnection();
    conn.Open();

    using var cmd = conn.CreateCommand();
    cmd.CommandText = $"SELECT p.Id, p.Nome, p.Email, p.Senha, p.Telefone FROM Pessoa p INNER JOIN Cliente c ON c.Id = p.Id WHERE p.Id = {atendimento.ClienteId}";

    using var reader = cmd.ExecuteReader();
    if (reader.Read())
    {
        atendimento.Cliente = new Cliente
        {
            Id       = reader.GetInt32(0),
            Nome     = reader.GetString(1),
            Email    = reader.GetString(2),
            Senha    = reader.GetString(3),
            Telefone = reader.IsDBNull(4) ? "Sem telefone" : reader.GetString(4)
        };
    }

    conn.Close();
}

    // Filtra atendimentos pelo dia selecionado
    barbeiro.Atendimentos = barbeiro.Atendimentos
        .Where(a => a.DataHora.Date == diaSelecionado)
        .ToList();

    var horarios = new List<DateTime>();

    var inicioManha = diaSelecionado.AddHours(8);
    var fimManha    = diaSelecionado.AddHours(12);
    var inicioTarde = diaSelecionado.AddHours(13);
    var fimTarde    = diaSelecionado.AddHours(19);

    for (var h = inicioManha; h < fimManha; h = h.AddMinutes(40))
        horarios.Add(h);

    for (var h = inicioTarde; h < fimTarde; h = h.AddMinutes(40))
        horarios.Add(h);

    ViewBag.Horarios = horarios;
    ViewBag.DiaSelecionado = diaSelecionado.ToString("yyyy-MM-dd");

    return View(barbeiro);
}
  public ActionResult Relatorio()
  {
    return View();
  }
}