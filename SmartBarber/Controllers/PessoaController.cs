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
            return RedirectToAction("Servicos", "Servico");
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
    public ActionResult Cadastro(Pessoa p)
    {
        if (!ModelState.IsValid)
        {
            return View(p);
        }

        db.Pessoa.Add(p);
        db.SaveChanges();
        return RedirectToAction("Login");
    }
}