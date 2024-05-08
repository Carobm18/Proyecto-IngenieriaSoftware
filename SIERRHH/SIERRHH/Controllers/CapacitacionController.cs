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
    public class CapacitacionController : Controller
    {
        private readonly AppBdContext _context;

        public CapacitacionController(AppBdContext context)
        {
            _context = context;
        }

        // GET: Capacitacion
        public async Task<IActionResult> Index()
        {
            return View(await _context.Capacitacion.ToListAsync());
        }

        // GET: Capacitacion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var capacitacion = await _context.Capacitacion
                .FirstOrDefaultAsync(m => m.IdCapacitacion == id);
            if (capacitacion == null)
            {
                return NotFound();
            }

            return View(capacitacion);
        }

        // GET: Capacitacion/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Capacitacion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCapacitacion,IdEmpleado,NombreCapacitacion,Lugar,Year,Estado")] Capacitacion capacitacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(capacitacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(capacitacion);
        }

        // GET: Capacitacion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var capacitacion = await _context.Capacitacion.FindAsync(id);
            if (capacitacion == null)
            {
                return NotFound();
            }
            return View(capacitacion);
        }

        // POST: Capacitacion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCapacitacion,IdEmpleado,NombreCapacitacion,Lugar,Year,Estado")] Capacitacion capacitacion)
        {
            if (id != capacitacion.IdCapacitacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(capacitacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CapacitacionExists(capacitacion.IdCapacitacion))
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
            return View(capacitacion);
        }

        // GET: Capacitacion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var capacitacion = await _context.Capacitacion
                .FirstOrDefaultAsync(m => m.IdCapacitacion == id);
            if (capacitacion == null)
            {
                return NotFound();
            }

            return View(capacitacion);
        }

        // POST: Capacitacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var capacitacion = await _context.Capacitacion.FindAsync(id);
            if (capacitacion != null)
            {
                _context.Capacitacion.Remove(capacitacion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CapacitacionExists(int id)
        {
            return _context.Capacitacion.Any(e => e.IdCapacitacion == id);
        }
    }
}
