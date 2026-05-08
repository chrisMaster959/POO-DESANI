using Microsoft.AspNetCore.Mvc;

public class ServicoController: Controller
{
  List<Servico> servicos = new List<Servico>
    {
        new Servico(1, "Corte de cabelo", 30.0),
        new Servico(2, "Barba", 20.0),
        new Servico(3, "Sobrancelha", 15.0),
        new Servico(4, "Corte + Barba", 45.0),
        new Servico(5, "Corte + Sobrancelha", 40.0)
    };

    public ActionResult Servicos()
    {
        return View(servicos);
    }

}