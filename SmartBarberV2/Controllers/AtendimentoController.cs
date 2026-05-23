using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class AtendimentoController : Controller
{
    private readonly DatabaseContext db;

    public AtendimentoController(DatabaseContext db)
    {
        this.db = db;
    }

    public ActionResult Atendimentos()
    {
        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

        if (usuarioId == null)
            return RedirectToAction("Login", "Pessoa");

        // Busca apenas os atendimentos do cliente logado
        var atendimentos = db.Atendimento
            .Include(a => a.Servicos)
            .Where(a => a.ClienteId == usuarioId)
            .ToList();

        // Busca o barbeiro de cada atendimento via SQL raw (evita bug TPT)
        foreach (var atendimento in atendimentos)
        {
            var conn = db.Database.GetDbConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = $@"
                SELECT p.Id, p.Nome, b.Logradouro, b.Nr_Logradouro, b.Bairro
                FROM Pessoa p
                INNER JOIN Barbeiro b ON b.Id = p.Id
                WHERE p.Id = {atendimento.BarbeiroId}";

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                atendimento.Barbeiro = new Barbeiro
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    Logradouro = reader.GetString(2),
                    Nr_Logradouro = reader.GetInt32(3),
                    Bairro = reader.GetString(4)
                };
            }

            conn.Close();
        }

        return View(atendimentos);
    }

    // GET: Editar dia e hora do atendimento
    public ActionResult Editar(int id)
    {
        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

        if (usuarioId == null)
            return RedirectToAction("Login", "Pessoa");

        var atendimento = db.Atendimento
            .FirstOrDefault(a => a.Id == id && a.ClienteId == usuarioId);

        if (atendimento == null)
            return NotFound();

        return View(atendimento);
    }

    // POST: Salvar nova data/hora
    [HttpPost]
    public ActionResult Editar(int id, DateTime novaDataHora)
    {
        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

        if (usuarioId == null)
            return RedirectToAction("Login", "Pessoa");

        var atendimento = db.Atendimento
            .FirstOrDefault(a => a.Id == id && a.ClienteId == usuarioId);

        if (atendimento == null)
            return NotFound();

        atendimento.DataHora = novaDataHora;
        db.SaveChanges();

        return RedirectToAction("Atendimentos");
    }

    // POST: Excluir atendimento
    [HttpPost, ActionName("Excluir")]
    public ActionResult Excluir(int id)
    {
        var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

        if (usuarioId == null)
            return RedirectToAction("Login", "Pessoa");

        var atendimento = db.Atendimento
            .FirstOrDefault(a => a.Id == id && a.ClienteId == usuarioId);

        if (atendimento != null)
        {
            db.Atendimento.Remove(atendimento);
            db.SaveChanges();
        }

        return RedirectToAction("Atendimentos");
    }
}