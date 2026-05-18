using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // =========================
        // VERIFICAÇÕES DE SESSÃO
        // =========================

        private bool UsuarioLogado()
        {
            return HttpContext.Session.GetString("UsuarioNome") != null;
        }

        private bool AdminLogado()
        {
            return HttpContext.Session.GetString("TipoUsuario") == "Admin";
        }

        // =========================
        // LISTA DE USUÁRIOS (ADMIN)
        // =========================

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            if (!AdminLogado())
            {
                return RedirectToAction("Index", "Login");
            }

            return View(await _context.Usuarios.ToListAsync());
        }

        // =========================
        // DETALHES (ADMIN)
        // =========================

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!AdminLogado())
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

        // =========================
        // CREATE (ADMIN)
        // =========================

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            if (!AdminLogado())
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,CPF,Email,Senha,Pontos,DataCadastro,TipoUsuario")] Usuario usuario)
        {
            if (!AdminLogado())
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                _context.Add(usuario);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }

        // =========================
        // EDIT (ADMIN)
        // =========================

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!AdminLogado())
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,CPF,Email,Senha,Pontos,DataCadastro,TipoUsuario")] Usuario usuario)
        {
            if (!AdminLogado())
            {
                return RedirectToAction("Index", "Login");
            }

            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }

        // =========================
        // DELETE (ADMIN)
        // =========================

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!AdminLogado())
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

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!AdminLogado())
            {
                return RedirectToAction("Index", "Login");
            }

            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // RANKING (QUALQUER LOGADO)
        // =========================

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

        // =========================
        // EXISTS
        // =========================

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}