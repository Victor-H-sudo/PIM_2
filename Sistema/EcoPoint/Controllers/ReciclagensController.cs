using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EcoPoint.Models;
using EcoPoint.Data;

namespace EcoPoint.Controllers
{
    public class ReciclagensController : ControllerBaseEcoPoint
    {
        public ReciclagensController(ApplicationDbContext context) : base(context)
        {
        }

        public IActionResult Index()
        {
            var reciclagens = _context.Reciclagens.ToList();
            return View(reciclagens);
        }

        public IActionResult Details(int id)
        {
            var reciclagem = _context.Reciclagens
                .FirstOrDefault(x => x.Id == id);

            if (reciclagem == null)
                return NotFound();

            return View(reciclagem);
        }

        // GET: Reciclagens/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] =
                new SelectList(_context.Usuarios, "Id", "CPF");

            ViewData["EcopontoId"] =
                new SelectList(_context.Ecopontos, "Id", "Nome");

            ViewData["TipoMaterialId"] =
                new SelectList(_context.TiposMaterial, "Id", "Nome");

            return View();
        }

        // POST: Reciclagens/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Reciclagem reciclagem)
        {
            if (ModelState.IsValid)
            {
                var tipoMaterial = _context.TiposMaterial
                    .FirstOrDefault(t => t.Id == reciclagem.TipoMaterialId);

                if (tipoMaterial != null)
                {
                    reciclagem.PontosGerados =
                        (int)(reciclagem.Peso * tipoMaterial.Multiplicador);
                }

                reciclagem.Data = DateTime.Now;

                var usuario = _context.Usuarios
                    .FirstOrDefault(u => u.Id == reciclagem.UsuarioId);

                if (usuario != null)
                {
                    usuario.Pontos += reciclagem.PontosGerados;
                }

                _context.Reciclagens.Add(reciclagem);

                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewData["UsuarioId"] =
                new SelectList(_context.Usuarios, "Id", "CPF", reciclagem.UsuarioId);

            ViewData["EcopontoId"] =
                new SelectList(_context.Ecopontos, "Id", "Nome", reciclagem.EcopontoId);

            ViewData["TipoMaterialId"] =
                new SelectList(_context.TiposMaterial, "Id", "Nome", reciclagem.TipoMaterialId);

            return View(reciclagem);
        }

        public IActionResult Delete(int id)
        {
            var reciclagem = _context.Reciclagens
                .FirstOrDefault(x => x.Id == id);

            if (reciclagem == null)
                return NotFound();

            _context.Reciclagens.Remove(reciclagem);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}