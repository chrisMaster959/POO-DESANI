using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class PessoaController : Controller
{
    private readonly DatabaseContext db;

    public PessoaController(DatabaseContext db)
    {
        this.db = db;
    }

    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Login(string Email, string Senha)
    {
        var pessoa = db.Pessoa.FirstOrDefault(p => p.Email == Email && p.Senha == Senha);
        if (pessoa != null)
        {
            HttpContext.Session.SetInt32("UsuarioId", pessoa.Id);
            HttpContext.Session.SetString("UsuarioNome", pessoa.Nome);
            HttpContext.Session.SetString("UsuarioEmail", pessoa.Email);
            
            return RedirectToAction("Atendimentos", "Atendimento");
        }

        ViewBag.Erro = "Email ou senha inválidos.";
        return View();
    }

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
}