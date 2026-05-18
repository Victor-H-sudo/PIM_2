using EcoPoint.Data;
using EcoPoint.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcoPoint.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u =>
                    u.Email == model.Email &&
                    u.Senha == model.Senha);

            if (usuario == null)
            {
                ViewBag.Erro = "Usuário ou senha inválidos";
                return View();
            }

            HttpContext.Session.SetString("UsuarioNome", usuario.Nome);
            HttpContext.Session.SetString("TipoUsuario", usuario.TipoUsuario);

            if (usuario.TipoUsuario == "Admin")
{
    return RedirectToAction("Index", "Usuarios");
}

if (usuario.TipoUsuario == "Empresa")
{
    return RedirectToAction("Index", "Reciclagens");
}

return RedirectToAction("Ranking", "Usuarios");
        }
        public IActionResult Logout()
{
    HttpContext.Session.Clear();

    return RedirectToAction("Index");
}

        }
    }