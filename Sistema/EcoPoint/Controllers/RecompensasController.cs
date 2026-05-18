using Microsoft.AspNetCore.Mvc;
using EcoPoint.Data;
using EcoPoint.Models;

namespace EcoPoint.Controllers
{
    public class RecompensasController : ControllerBaseEcoPoint
    {
        public RecompensasController(ApplicationDbContext context) : base(context)
        {
        }

        public IActionResult Index()
        {
            var recompensas = _context.Recompensas.ToList();
            return View(recompensas);
        }

        public IActionResult Details(int id)
        {
            var recompensa = _context.Recompensas.FirstOrDefault(x => x.Id == id);
            if (recompensa == null) return NotFound();

            return View(recompensa);
        }
        public IActionResult Loja()
{
    if (HttpContext.Session.GetString("UsuarioNome") == null)
    {
        return RedirectToAction("Index", "Login");
    }

    var recompensas = _context.Recompensas.ToList();

    return View(recompensas);
}
        public IActionResult Resgatar(int id)
{
    if (HttpContext.Session.GetString("UsuarioNome") == null)
    {
        return RedirectToAction("Index", "Login");
    }

    var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

    if (usuarioId == null)
    {
        return RedirectToAction("Index", "Login");
    }

    var usuario = _context.Usuarios
        .FirstOrDefault(u => u.Id == usuarioId);

    var recompensa = _context.Recompensas
        .FirstOrDefault(r => r.Id == id);

    if (usuario == null || recompensa == null)
    {
        return NotFound();
    }

    if (usuario.Pontos < recompensa.PontosNecessarios)
    {
        TempData["Erro"] = "Pontos insuficientes para resgatar esta recompensa.";
        return RedirectToAction("Loja");
    }

    usuario.Pontos -= recompensa.PontosNecessarios;

    var resgate = new Resgate
    {
        UsuarioId = usuario.Id,
        RecompensaId = recompensa.Id,
        DataResgate = DateTime.Now
    };

    _context.Resgates.Add(resgate);
    _context.SaveChanges();

    TempData["Sucesso"] = "Recompensa resgatada com sucesso!";

    return RedirectToAction("Loja");
}
    }
}