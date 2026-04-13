using Microsoft.AspNetCore.Mvc;


public class TodoController : Controller
{
    // atributo:
    private static List<Todo> tasks = new List<Todo>();
    
    // http://localhost:1234/todo/index
    public ActionResult Index()
    {
        // /Views/Todo/Index.cshtml
        return View(tasks);
    }
    [HttpGet]
    public ActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public ActionResult Create(Todo t)
    {
        tasks.Add(t);
        return View();
        //return RedirectToAction("Index");
    }
}