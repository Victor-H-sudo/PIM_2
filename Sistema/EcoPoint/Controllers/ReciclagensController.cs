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
public class ReciclagensController : Controller
{
private readonly ApplicationDbContext _context;

    public ReciclagensController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ====================================
    // VERIFICAÇÃO DE PERMISSÃO
    // ====================================

    private bool EmpresaOuAdmin()
    {
        var tipo = HttpContext.Session.GetString("TipoUsuario");

        return tipo == "Admin" || tipo == "Empresa";
    }

    // ====================================
    // INDEX
    // ====================================

    // GET: Reciclagens
    public async Task<IActionResult> Index()
    {
        if (!EmpresaOuAdmin())
        {
            return RedirectToAction("Index", "Login");
        }

        var applicationDbContext = _context.Reciclagens
            .Include(r => r.Ecoponto)
            .Include(r => r.TipoMaterial)
            .Include(r => r.Usuario);

        return View(await applicationDbContext.ToListAsync());
    }

    // ====================================
    // DETAILS
    // ====================================

    // GET: Reciclagens/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (!EmpresaOuAdmin())
        {
            return RedirectToAction("Index", "Login");
        }

        if (id == null)
        {
            return NotFound();
        }

        var reciclagem = await _context.Reciclagens
            .Include(r => r.Ecoponto)
            .Include(r => r.TipoMaterial)
            .Include(r => r.Usuario)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (reciclagem == null)
        {
            return NotFound();
        }

        return View(reciclagem);
    }

    // ====================================
    // CREATE GET
    // ====================================

    // GET: Reciclagens/Create
    public IActionResult Create()
    {
        if (!EmpresaOuAdmin())
        {
            return RedirectToAction("Index", "Login");
        }

        ViewData["EcopontoId"] =
            new SelectList(_context.Ecopontos, "Id", "CNPJ");

        ViewData["TipoMaterialId"] =
            new SelectList(_context.TiposMaterial, "Id", "Nome");

        ViewData["UsuarioId"] =
            new SelectList(_context.Usuarios, "Id", "CPF");

        return View();
    }

    // ====================================
    // CREATE POST
    // ====================================

    // POST: Reciclagens/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,UsuarioId,EcopontoId,TipoMaterialId,Peso,FotoMaterial,FotoPeso")]
        Reciclagem reciclagem)
    {
        if (!EmpresaOuAdmin())
        {
            return RedirectToAction("Index", "Login");
        }

        if (ModelState.IsValid)
        {
            var tipoMaterial = await _context.TiposMaterial
                .FirstOrDefaultAsync(t => t.Id == reciclagem.TipoMaterialId);

            if (tipoMaterial != null)
            {
                reciclagem.PontosGerados =
                    (int)(reciclagem.Peso * tipoMaterial.Multiplicador);
            }

            reciclagem.Data = DateTime.Now;

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == reciclagem.UsuarioId);

            if (usuario != null)
            {
                usuario.Pontos += reciclagem.PontosGerados;
            }

            _context.Add(reciclagem);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewData["EcopontoId"] =
            new SelectList(_context.Ecopontos, "Id", "CNPJ");

        ViewData["TipoMaterialId"] =
            new SelectList(_context.TiposMaterial, "Id", "Nome");

        ViewData["UsuarioId"] =
            new SelectList(_context.Usuarios, "Id", "CPF");

        return View(reciclagem);
    }

    // ====================================
    // EDIT GET
    // ====================================

    // GET: Reciclagens/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (!EmpresaOuAdmin())
        {
            return RedirectToAction("Index", "Login");
        }

        if (id == null)
        {
            return NotFound();
        }

        var reciclagem = await _context.Reciclagens.FindAsync(id);

        if (reciclagem == null)
        {
            return NotFound();
        }

        ViewData["EcopontoId"] =
            new SelectList(_context.Ecopontos, "Id", "CNPJ", reciclagem.EcopontoId);

        ViewData["TipoMaterialId"] =
            new SelectList(_context.TiposMaterial, "Id", "Nome", reciclagem.TipoMaterialId);

        ViewData["UsuarioId"] =
            new SelectList(_context.Usuarios, "Id", "CPF", reciclagem.UsuarioId);

        return View(reciclagem);
    }

    // ====================================
    // EDIT POST
    // ====================================

    // POST: Reciclagens/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,UsuarioId,EcopontoId,TipoMaterialId,Peso,PontosGerados,FotoMaterial,FotoPeso")]
        Reciclagem reciclagem)
    {
        if (!EmpresaOuAdmin())
        {
            return RedirectToAction("Index", "Login");
        }

        if (id != reciclagem.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(reciclagem);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReciclagemExists(reciclagem.Id))
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

        ViewData["EcopontoId"] =
            new SelectList(_context.Ecopontos, "Id", "CNPJ", reciclagem.EcopontoId);

        ViewData["TipoMaterialId"] =
            new SelectList(_context.TiposMaterial, "Id", "Nome", reciclagem.TipoMaterialId);

        ViewData["UsuarioId"] =
            new SelectList(_context.Usuarios, "Id", "CPF", reciclagem.UsuarioId);

        return View(reciclagem);
    }

    // ====================================
    // DELETE GET
    // ====================================

    // GET: Reciclagens/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (!EmpresaOuAdmin())
        {
            return RedirectToAction("Index", "Login");
        }

        if (id == null)
        {
            return NotFound();
        }

        var reciclagem = await _context.Reciclagens
            .Include(r => r.Ecoponto)
            .Include(r => r.TipoMaterial)
            .Include(r => r.Usuario)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (reciclagem == null)
        {
            return NotFound();
        }

        return View(reciclagem);
    }

    // ====================================
    // DELETE POST
    // ====================================

    // POST: Reciclagens/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (!EmpresaOuAdmin())
        {
            return RedirectToAction("Index", "Login");
        }

        var reciclagem = await _context.Reciclagens.FindAsync(id);

        if (reciclagem != null)
        {
            _context.Reciclagens.Remove(reciclagem);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // ====================================
    // EXISTS
    // ====================================

    private bool ReciclagemExists(int id)
    {
        return _context.Reciclagens.Any(e => e.Id == id);
    }
}


}
