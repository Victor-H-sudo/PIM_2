using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcoPoint.Data;
using EcoPoint.Models;

namespace EcoPoint.Controllers
{
    public class TiposMaterialController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TiposMaterialController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TiposMaterial
        public async Task<IActionResult> Index()
        {
            return View(await _context.TiposMaterial.ToListAsync());
        }

        // GET: TiposMaterial/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoMaterial = await _context.TiposMaterial
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoMaterial == null)
            {
                return NotFound();
            }

            return View(tipoMaterial);
        }

        // GET: TiposMaterial/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TiposMaterial/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Multiplicador")] TipoMaterial tipoMaterial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoMaterial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoMaterial);
        }

        // GET: TiposMaterial/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoMaterial = await _context.TiposMaterial.FindAsync(id);
            if (tipoMaterial == null)
            {
                return NotFound();
            }
            return View(tipoMaterial);
        }

        // POST: TiposMaterial/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Multiplicador")] TipoMaterial tipoMaterial)
        {
            if (id != tipoMaterial.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoMaterial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoMaterialExists(tipoMaterial.Id))
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
            return View(tipoMaterial);
        }

        // GET: TiposMaterial/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoMaterial = await _context.TiposMaterial
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoMaterial == null)
            {
                return NotFound();
            }

            return View(tipoMaterial);
        }

        // POST: TiposMaterial/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoMaterial = await _context.TiposMaterial.FindAsync(id);
            if (tipoMaterial != null)
            {
                _context.TiposMaterial.Remove(tipoMaterial);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoMaterialExists(int id)
        {
            return _context.TiposMaterial.Any(e => e.Id == id);
        }
    }
}
