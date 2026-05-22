using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

public class AtendimentoController : Controller
{
    private readonly DatabaseContext db;

    public AtendimentoController(DatabaseContext db)
    {
        this.db = db;
    }

    // GET: Atendimento
    public ActionResult Atendimentos()
    {
        var atendimentos = db.Atendimento.ToList();
        return View(atendimentos);
    }

    // GET: Atendimento/Details/5
    public ActionResult Details(int id)
    {
        var atendimento = db.Atendimento.FirstOrDefault(a => a.Id == id);
        if (atendimento == null)
            return NotFound();

        return View(atendimento);
    }

    // GET: Atendimento/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: Atendimento/Create
    [HttpPost]
    public ActionResult Create(Atendimento atendimento)
    {
        if (!ModelState.IsValid)
            return View(atendimento);

        db.Atendimento.Add(atendimento);
        db.SaveChanges();
        return RedirectToAction("Index");
    }

    // GET: Atendimento/Edit/5
    public ActionResult Edit(int id)
    {
        var atendimento = db.Atendimento.FirstOrDefault(a => a.Id == id);
        if (atendimento == null)
            return NotFound();

        return View(atendimento);
    }

    // POST: Atendimento/Edit/5
    [HttpPost]
    public ActionResult Edit(int id, Atendimento atendimento)
    {
        if (id != atendimento.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(atendimento);

        db.Atendimento.Update(atendimento);
        db.SaveChanges();
        return RedirectToAction("Index");
    }

    // GET: Atendimento/Delete/5
    public ActionResult Delete(int id)
    {
        var atendimento = db.Atendimento.FirstOrDefault(a => a.Id == id);
        if (atendimento == null)
            return NotFound();

        return View(atendimento);
    }

    // POST: Atendimento/Delete/5
    [HttpPost]
    public ActionResult DeleteConfirmed(int id)
    {
        var atendimento = db.Atendimento.FirstOrDefault(a => a.Id == id);
        if (atendimento != null)
        {
            db.Atendimento.Remove(atendimento);
            db.SaveChanges();
        }

        return RedirectToAction("Index");
    }
}
