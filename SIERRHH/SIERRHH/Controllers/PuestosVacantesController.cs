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
    public class PuestosVacantesController : Controller
    {
        private readonly AppBdContext _context;

        public PuestosVacantesController(AppBdContext context)
        {
            _context = context; 
        }

        // GET: PuestosVacantes
        public async Task<IActionResult> Index()
        {
            return View(await _context.PuestosVacantes.ToListAsync());
        }

        // GET: PuestosVacantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var puestosVacantes = await _context.PuestosVacantes
                .FirstOrDefaultAsync(m => m.IdPuesto == id);
            if (puestosVacantes == null)
            {
                return NotFound();
            }

            return View(puestosVacantes);
        }

        // GET: PuestosVacantes/Create
        public IActionResult Create()
        {
            ViewBag.Sectores = _context.Sector.ToList();
            return View();
        }

        // POST: PuestosVacantes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPuesto,Descripcion,IdSector,Estado,FechaPublicacion,Modalidad")] PuestosVacantes puestosVacantes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(puestosVacantes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(puestosVacantes);
        }

        // GET: PuestosVacantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var puestosVacantes = await _context.PuestosVacantes.FindAsync(id);
            if (puestosVacantes == null)
            {
                return NotFound();
            }
            ViewBag.Sectores = _context.Sector.ToList();
            return View(puestosVacantes);
        }

        // POST: PuestosVacantes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPuesto,Descripcion,IdSector,Estado,FechaPublicacion,Modalidad")] PuestosVacantes puestosVacantes)
        {
            if (id != puestosVacantes.IdPuesto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(puestosVacantes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PuestosVacantesExists(puestosVacantes.IdPuesto))
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
            return View(puestosVacantes);
        }

        // GET: PuestosVacantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var puestosVacantes = await _context.PuestosVacantes
                .FirstOrDefaultAsync(m => m.IdPuesto == id);
            if (puestosVacantes == null)
            {
                return NotFound();
            }

            return View(puestosVacantes);
        }

        // POST: PuestosVacantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var puestosVacantes = await _context.PuestosVacantes.FindAsync(id);
            if (puestosVacantes != null)
            {
                _context.PuestosVacantes.Remove(puestosVacantes);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PuestosVacantesExists(int id)
        {
            return _context.PuestosVacantes.Any(e => e.IdPuesto == id);
        }

        //ver puestos vacantes
        public async Task<IActionResult> PuestosVacantes()
        {
            return View(await _context.PuestosVacantes.ToListAsync());
        }
    }
}
