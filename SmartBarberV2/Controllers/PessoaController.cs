using Microsoft.AspNetCore.Mvc;
using System.Linq;

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

        db.Cliente.Add(c);

        db.SaveChanges();

        return RedirectToAction("Login");
    }

    // CADASTRO BARBEIRO

    [HttpGet]
    public ActionResult CadastroBarbeiro()
    {
        ViewBag.Ceps = db.Cep.ToList();

        return View();
    }

    [HttpPost]
    public ActionResult CadastroBarbeiro(Barbeiro b)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Ceps = db.Cep.ToList();

            return View(b);
        }

        db.Barbeiro.Add(b);

        db.SaveChanges();

        HttpContext.Session.SetInt32("UsuarioId", b.Id);
        HttpContext.Session.SetString("UsuarioNome", b.Nome);
        HttpContext.Session.SetString("UsuarioEmail", b.Email);

        return RedirectToAction("Escolha", "Servico");
    }

    [HttpPost]
    public ActionResult Sair()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Pessoa");
    }
}