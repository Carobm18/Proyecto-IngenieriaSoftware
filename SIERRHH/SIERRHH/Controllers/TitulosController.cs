using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIERRHH.Models;

namespace SIERRHH.Controllers
{
    public class TitulosController : Controller
    {
        private readonly AppBdContext _context;

        public TitulosController(AppBdContext context)
        {
            _context = context;
        }

        // GET: Titulos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Titulos.ToListAsync());
        }

        // GET: Titulos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var titulos = await _context.Titulos
                .FirstOrDefaultAsync(m => m.IdTitulo == id);
            if (titulos == null)
            {
                return NotFound();
            }

            return View(titulos);
        }

        // GET: Titulos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Titulos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTitulo,IdEmpleado,Descripcion,CentroEstudio,IdSector,IdGrado")] Titulos titulos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(titulos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(titulos);
        }

        // GET: Titulos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var titulos = await _context.Titulos.FindAsync(id);
            if (titulos == null)
            {
                return NotFound();
            }
            return View(titulos);
        }

        // POST: Titulos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTitulo,IdEmpleado,Descripcion,CentroEstudio,IdSector,IdGrado")] Titulos titulos)
        {
            if (id != titulos.IdTitulo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(titulos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TitulosExists(titulos.IdTitulo))
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
            return View(titulos);
        }

        // GET: Titulos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var titulos = await _context.Titulos
                .FirstOrDefaultAsync(m => m.IdTitulo == id);
            if (titulos == null)
            {
                return NotFound();
            }

            return View(titulos);
        }

        // POST: Titulos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var titulos = await _context.Titulos.FindAsync(id);
            if (titulos != null)
            {
                _context.Titulos.Remove(titulos);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TitulosExists(int id)
        {
            return _context.Titulos.Any(e => e.IdTitulo == id);
        }
    }
}
