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
    public class CertificacionController : Controller
    {
        private readonly AppBdContext _context;

        public CertificacionController(AppBdContext context)
        {
            _context = context;
        }

        // GET: Certificacion
        public async Task<IActionResult> Index()
        {
            return View(await _context.Certificacion.ToListAsync());
        }

        // GET: Certificacion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var certificacion = await _context.Certificacion
                .FirstOrDefaultAsync(m => m.IdCertificado == id);
            if (certificacion == null)
            {
                return NotFound();
            }

            return View(certificacion);
        }

        // GET: Certificacion/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Certificacion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCertificado,IdEmpleado,NombreCertificacion,Entidad,IdSector")] Certificacion certificacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(certificacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(certificacion);
        }

        // GET: Certificacion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var certificacion = await _context.Certificacion.FindAsync(id);
            if (certificacion == null)
            {
                return NotFound();
            }
            return View(certificacion);
        }

        // POST: Certificacion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCertificado,IdEmpleado,NombreCertificacion,Entidad,IdSector")] Certificacion certificacion)
        {
            if (id != certificacion.IdCertificado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(certificacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CertificacionExists(certificacion.IdCertificado))
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
            return View(certificacion);
        }

        // GET: Certificacion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var certificacion = await _context.Certificacion
                .FirstOrDefaultAsync(m => m.IdCertificado == id);
            if (certificacion == null)
            {
                return NotFound();
            }

            return View(certificacion);
        }

        // POST: Certificacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var certificacion = await _context.Certificacion.FindAsync(id);
            if (certificacion != null)
            {
                _context.Certificacion.Remove(certificacion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CertificacionExists(int id)
        {
            return _context.Certificacion.Any(e => e.IdCertificado == id);
        }
    }
}
