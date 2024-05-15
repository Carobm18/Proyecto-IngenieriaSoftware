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
    public class PerfilAptitudesController : Controller
    {
        private readonly AppBdContext _context;

        public PerfilAptitudesController(AppBdContext context)
        {
            _context = context;
        }

        // GET: PerfilAptitudes
        public async Task<IActionResult> Index()
        {
            return View(await _context.PerfilAptitudes.ToListAsync());
        }

        // GET: PerfilAptitudes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var perfilAptitudes = await _context.PerfilAptitudes
                .FirstOrDefaultAsync(m => m.IdAptitudes == id);
            if (perfilAptitudes == null)
            {
                return NotFound();
            }

            return View(perfilAptitudes);
        }

        // GET: PerfilAptitudes/Create
        public IActionResult Create(int? id)
        {

            var perfilAptitudes= new PerfilAptitudes();

            perfilAptitudes.IdEmpleado = (int)id;



            ViewBag.Aptitudes = _context.Aptitudes.ToList();
            return View(perfilAptitudes);
        }

        // POST: PerfilAptitudes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEmpleado,IdAptitudes")] PerfilAptitudes perfilAptitudes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(perfilAptitudes);
                await _context.SaveChangesAsync();
                return RedirectToAction("MiPerfil", "PerfilProfesional");
            }
            return View(perfilAptitudes);
        }

        // GET: PerfilAptitudes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var perfilAptitudes = await _context.PerfilAptitudes.FindAsync(id);
            if (perfilAptitudes == null)
            {
                return NotFound();
            }
            return View(perfilAptitudes);
        }

        // POST: PerfilAptitudes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEmpleado,IdAptitudes")] PerfilAptitudes perfilAptitudes)
        {
            if (id != perfilAptitudes.IdAptitudes)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(perfilAptitudes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerfilAptitudesExists(perfilAptitudes.IdAptitudes))
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
            return View(perfilAptitudes);
        }

        // GET: PerfilAptitudes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var perfilAptitudes = await _context.PerfilAptitudes
                .FirstOrDefaultAsync(m => m.IdAptitudes == id);
            if (perfilAptitudes == null)
            {
                return NotFound();
            }

            return View(perfilAptitudes);
        }

        // POST: PerfilAptitudes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var perfilAptitudes = await _context.PerfilAptitudes.FindAsync(id);
            if (perfilAptitudes != null)
            {
                _context.PerfilAptitudes.Remove(perfilAptitudes);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PerfilAptitudesExists(int id)
        {
            return _context.PerfilAptitudes.Any(e => e.IdAptitudes == id);
        }
    }
}
