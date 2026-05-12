using Microsoft.AspNetCore.Mvc;

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
  public ActionResult Cadastro(Barbeiro b)
  {
    if (!ModelState.IsValid)
    {
        return View(b);
    }

    db.Barbeiro.Add(b);

    db.SaveChanges();

    return RedirectToAction("Controle");
  }

  public ActionResult Controle()
  {
    var barbeiros = db.Barbeiro.ToList();
    return View(barbeiros);
  }

  public ActionResult Relatorio()
  {
    return View();
  }
}