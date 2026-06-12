using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class PessoaController : Controller
{
private readonly DatabaseContext db;


public PessoaController(DatabaseContext db)
{
    this.db = db;
}

// LOGIN CLIENTE

[HttpGet]
public ActionResult Login()
{
    return View();
}

[HttpPost]
public ActionResult Login(
    string Email,
    string Senha)
{
    var pessoa = db.Pessoa
        .FirstOrDefault(p =>
            p.Email == Email &&
            p.Senha == Senha);

    if (pessoa == null)
    {
        ViewBag.Erro = "Email ou senha inválidos.";

        return View();
    }

    HttpContext.Session.SetInt32("UsuarioId", pessoa.Id);
    HttpContext.Session.SetString("UsuarioNome", pessoa.Nome);
    HttpContext.Session.SetString("UsuarioEmail", pessoa.Email);

    return RedirectToAction("Atendimentos", "Atendimento");
}

// LOGIN BARBEIRO

[HttpGet]
public ActionResult LoginBarbeiro()
{
    return View();
}

[HttpPost]
public ActionResult LoginBarbeiro(
    string Email,
    string Senha)
{
    var pessoa = db.Pessoa
        .FirstOrDefault(p =>
            p.Email == Email &&
            p.Senha == Senha);

    if (pessoa == null)
    {
        ViewBag.Erro = "Email ou senha inválidos.";

        return View();
    }

    // verifica se existe barbeiro com esse id
    var barbeiroExiste = db.Barbeiro
        .Any(b => b.Id == pessoa.Id);

    if (!barbeiroExiste)
    {
        ViewBag.Erro = "Você não é um barbeiro.";

        return View();
    }

    HttpContext.Session.SetInt32("UsuarioId", pessoa.Id);
    HttpContext.Session.SetString("UsuarioNome", pessoa.Nome);
    HttpContext.Session.SetString("UsuarioEmail", pessoa.Email);

    return RedirectToAction("Controle", "Barbeiro");
}

// CADASTRO CLIENTE

[HttpGet]
public ActionResult Cadastro()
{
    return View();
}

[HttpPost]
public ActionResult Cadastro(Cliente c)
{
    if (!ModelState.IsValid)
    {
        return View(c);
    }

    bool emailExiste = db.Pessoa
        .Any(p => p.Email.ToLower() == c.Email.ToLower());

    if (emailExiste)
    {
        ViewBag.Erro = "Este e-mail já está cadastrado.";
        return View(c);
    }

    db.Cliente.Add(c);

    db.SaveChanges();

    return RedirectToAction("Login");
}

[HttpPost]
public ActionResult Sair()
{
    HttpContext.Session.Clear();
    return RedirectToAction("Login", "Pessoa");
}


[HttpPost]
public ActionResult TornarCliente()
{
    var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

    if (usuarioId == null)
        return RedirectToAction("Login", "Pessoa");

    // verifica se já é cliente
    var clienteExiste = db.Cliente.Any(c => c.Id == usuarioId);

    if (!clienteExiste)
    {
        // INSERT SOMENTE na tabela derivada (TPT)
        db.Database.ExecuteSqlRaw(@"
            INSERT INTO Cliente (Id) VALUES ({0})
        ", usuarioId);
    }

    return RedirectToAction("Atendimentos", "Atendimento");
}

}
