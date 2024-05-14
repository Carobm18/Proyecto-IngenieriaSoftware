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
    public class AptitudesController : Controller
    {
        private readonly AppBdContext _context;

        public AptitudesController(AppBdContext context)
        {
            _context = context;
        }

        // GET: Aptitudes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Aptitudes.ToListAsync());
        }

        // GET: Aptitudes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aptitudes = await _context.Aptitudes
                .FirstOrDefaultAsync(m => m.IdAptitud == id);
            if (aptitudes == null)
            {
                return NotFound();
            }

            return View(aptitudes);
        }

        // GET: Aptitudes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Aptitudes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAptitud,Descripcion,Puntaje,Estado")] Aptitudes aptitudes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aptitudes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aptitudes);
        }

        // GET: Aptitudes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aptitudes = await _context.Aptitudes.FindAsync(id);
            if (aptitudes == null)
            {
                return NotFound();
            }
            return View(aptitudes);
        }

        // POST: Aptitudes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAptitud,Descripcion,Puntaje,Estado")] Aptitudes aptitudes)
        {
            if (id != aptitudes.IdAptitud)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aptitudes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AptitudesExists(aptitudes.IdAptitud))
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
            return View(aptitudes);
        }

        // GET: Aptitudes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aptitudes = await _context.Aptitudes
                .FirstOrDefaultAsync(m => m.IdAptitud == id);
            if (aptitudes == null)
            {
                return NotFound();
            }

            return View(aptitudes);
        }

        // POST: Aptitudes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aptitudes = await _context.Aptitudes.FindAsync(id);
            if (aptitudes != null)
            {
                _context.Aptitudes.Remove(aptitudes);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AptitudesExists(int id)
        {
            return _context.Aptitudes.Any(e => e.IdAptitud == id);
        }
    }
}
