using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public async Task<IActionResult> Index(int? id)
        {
            var capacitaciones = listasCapacitacion((int)id);
            return View(capacitaciones);
        }

        private List<Capacitacion> listasCapacitacion(int id)
        {
            var listaCapacitacion = _context.Capacitacion.FromSql($"EXEC Sp_Cns_ListaCapacitacion @idEmpleado={id}").ToList();
            return listaCapacitacion;
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
            int idEmpleado = ObtenerIdEmpleadoAutenticado();

            // Crea una nueva instancia de PuestosDesempenados con el IdEmpleado asignado
            var capacitacion = new Capacitacion
            {
                IdEmpleado = idEmpleado
            };

            return View(capacitacion);
        }

        private int ObtenerIdEmpleadoAutenticado()
        {
            // Aquí puedes acceder al IdEmpleado desde las reclamaciones (claims) del usuario autenticado
            var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            var idEmpleadoClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (idEmpleadoClaim != null && int.TryParse(idEmpleadoClaim.Value, out int idEmpleado))
            {
                return idEmpleado;
            }

            return 0; // Valor por defecto si no se puede obtener el IdEmpleado
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
                return RedirectToAction("MiPerfil", "PerfilProfesional");
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
                return RedirectToAction("MiPerfil", "PerfilProfesional");
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
            _context.Capacitacion.Remove(capacitacion);
            await _context.SaveChangesAsync();

            return RedirectToAction("MiPerfil", "PerfilProfesional");
        }

        // POST: Capacitacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var capacitacion = await _context.Capacitacion.FindAsync(id);
            if (capacitacion != null)
            {
              
            }

          
            return RedirectToAction("MiPerfil", "PerfilProfesional");
        }

        private bool CapacitacionExists(int id)
        {
            return _context.Capacitacion.Any(e => e.IdCapacitacion == id);
        }
    }
}
