using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using EcoPoint.Data;
using EcoPoint.Models;

namespace EcoPoint.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool UsuarioLogado()
        {
            return HttpContext.Session.GetString("UsuarioNome") != null;
        }

        private bool Admin()
        {
            return HttpContext.Session.GetString("TipoUsuario") == "Admin";
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            if (!UsuarioLogado())
            {
                return RedirectToAction("Index", "Login");
            }

            return View(await _context.Usuarios.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!UsuarioLogado())
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            if (!Admin())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Nome,CPF,Email,Senha,Pontos,DataCadastro,TipoUsuario")]
            Usuario usuario)
        {
            if (!Admin())
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                _context.Add(usuario);

                await _context.SaveChangesAsync();

                TempData["Sucesso"] = "Usuário criado com sucesso!";

                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!Admin())
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!Admin())
            {
                return RedirectToAction("Index", "Home");
            }

            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();

            TempData["Sucesso"] = "Usuário removido com sucesso!";

            return RedirectToAction(nameof(Index));
        }

        // Ranking
        public async Task<IActionResult> Ranking()
        {
            if (!UsuarioLogado())
            {
                return RedirectToAction("Index", "Login");
            }

            var usuarios = await _context.Usuarios
                .OrderByDescending(u => u.Pontos)
                .ToListAsync();

            return View(usuarios);
        }

        // GET: Usuarios/CriarEmpresa
        public IActionResult CriarEmpresa()
        {
            if (!Admin())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: Usuarios/CriarEmpresa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CriarEmpresa(Usuario usuario)
        {
            if (!Admin())
            {
                return RedirectToAction("Index", "Home");
            }

            usuario.TipoUsuario = "Empresa";
            usuario.Pontos = 0;
            usuario.DataCadastro = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Usuarios.Add(usuario);

                _context.SaveChanges();

                TempData["Sucesso"] =
                    "Empresa cadastrada com sucesso!";

                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}