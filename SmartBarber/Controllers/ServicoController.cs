using Microsoft.AspNetCore.Mvc;

public class ServicoController: Controller
{
  private readonly DatabaseContext db;

  public ServicoController(DatabaseContext db)
  {
    this.db = db;
  }

  public ActionResult Servicos()
  {
    var servicos = db.Servico.ToList();
 
    return View(servicos);
  }
}