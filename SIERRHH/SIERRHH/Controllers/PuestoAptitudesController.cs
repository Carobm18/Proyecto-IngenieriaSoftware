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
    public class PuestoAptitudesController : Controller
    {
        private readonly AppBdContext _context;

        public PuestoAptitudesController(AppBdContext context)
        {
            _context = context;
        }

        // GET: PuestoAptitudes
        public async Task<IActionResult> Index()
        {
            return View(await _context.PuestoAptitudes.ToListAsync());
        }

        // GET: PuestoAptitudes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var puestoAptitudes = await _context.PuestoAptitudes
                .FirstOrDefaultAsync(m => m.IdAptitudes == id);
                

            if (puestoAptitudes == null)
            {
                return NotFound();
            }

            return View(puestoAptitudes);
        }

        // GET: PuestoAptitudes/Create
        public IActionResult Create(int? id)
        {
            var puestoAptitudes = new PuestoAptitudes();

           puestoAptitudes.IdPuesto = (int)id;

            
            
            ViewBag.Aptitudes = _context.Aptitudes.ToList();
            return View(puestoAptitudes);
        }

        // POST: PuestoAptitudes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAptitudes,IdPuesto")] PuestoAptitudes puestoAptitudes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(puestoAptitudes);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "PuestosVacantes", new { id = puestoAptitudes.IdPuesto });

            }
            return View(puestoAptitudes);
        }

        // GET: PuestoAptitudes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var puestoAptitudes = await _context.PuestoAptitudes.FindAsync(id);
            if (puestoAptitudes == null)
            {
                return NotFound();
            }
            return View(puestoAptitudes);
        }

        // POST: PuestoAptitudes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAptitudes,IdPuesto")] PuestoAptitudes puestoAptitudes)
        {
            if (id != puestoAptitudes.IdAptitudes)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(puestoAptitudes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PuestoAptitudesExists(puestoAptitudes.IdAptitudes))
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
            return View(puestoAptitudes);
        }

        // GET: PuestoAptitudes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var puestoAptitudes = await _context.PuestoAptitudes
                .FirstOrDefaultAsync(m => m.IdAptitudes == id);
            if (puestoAptitudes == null)
            {
                return NotFound();
            }

            return View(puestoAptitudes);
        }

        // POST: PuestoAptitudes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var puestoAptitudes = await _context.PuestoAptitudes.FindAsync(id);
            if (puestoAptitudes != null)
            {
                _context.PuestoAptitudes.Remove(puestoAptitudes);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PuestoAptitudesExists(int id)
        {
            return _context.PuestoAptitudes.Any(e => e.IdAptitudes == id);
        }
    }
}
