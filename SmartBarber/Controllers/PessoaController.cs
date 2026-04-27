using Microsoft.AspNetCore.Mvc;

public class PessoaController : Controller
{
    
    private DatabaseContext db;
    
    public PessoaController(DatabaseContext db)
    {
        this.db = db;
    }
    public ActionResult Login()
    {
        // /Views/Pessoa/Login.cshtml
        return View(db.Pessoa.ToList());
    }

    [HttpGet]
    public ActionResult Cadastro()
    {
        return View();
    }

    // [HttpPost]
    // public ActionResult Cadastro(Pessoa p)
    // {
    //     p.Codigo = Guid.NewGuid().ToString();
    //     db.Pessoa.Add(p);
    //     db.SaveChanges();
    // }
}