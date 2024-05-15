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
    public class PuestosDesempenadosController : Controller
    {
        private readonly AppBdContext _context;

        public PuestosDesempenadosController(AppBdContext context)
        {
            _context = context;
        }

        // GET: PuestosDesempenados
        public async Task<IActionResult> Index()
        {
            return View(await _context.PuestosDesempenados.ToListAsync());
        }

        // GET: PuestosDesempenados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var puestosDesempenados = await _context.PuestosDesempenados
                .FirstOrDefaultAsync(m => m.IdPuestoDesempenado == id);
            if (puestosDesempenados == null)
            {
                return NotFound();
            }

            return View(puestosDesempenados);
        }

        // GET: PuestosDesempenados/Create
        public IActionResult Create()
        {
            int idEmpleado = ObtenerIdEmpleadoAutenticado();

            // Crea una nueva instancia de PuestosDesempenados con el IdEmpleado asignado
            var puestosDesempenados = new PuestosDesempenados
            {
                IdEmpleado = idEmpleado
            };

            return View(puestosDesempenados);
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

        // POST: PuestosDesempenados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPuestoDesempenado,IdEmpleado,Descripcion,Empresa,Tiempo")] PuestosDesempenados puestosDesempenados)
        {
            if (ModelState.IsValid)
            {
                _context.Add(puestosDesempenados);
                await _context.SaveChangesAsync();
                return RedirectToAction("MiPerfil", "PerfilProfesional");
            }
            return View(puestosDesempenados);
        }

        // GET: PuestosDesempenados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var puestosDesempenados = await _context.PuestosDesempenados.FindAsync(id);
            if (puestosDesempenados == null)
            {
                return NotFound();
            }
            return View(puestosDesempenados);
        }

        // POST: PuestosDesempenados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPuestoDesempenado,IdEmpleado,Descripcion,Empresa,Tiempo")] PuestosDesempenados puestosDesempenados)
        {
            if (id != puestosDesempenados.IdPuestoDesempenado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(puestosDesempenados);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PuestosDesempenadosExists(puestosDesempenados.IdPuestoDesempenado))
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
            return View(puestosDesempenados);
        }

        // GET: PuestosDesempenados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var puestosDesempenados = await _context.PuestosDesempenados
                .FirstOrDefaultAsync(m => m.IdPuestoDesempenado == id);
            if (puestosDesempenados == null)
            {
                return NotFound();
            }

            return View(puestosDesempenados);
        }

        // POST: PuestosDesempenados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var puestosDesempenados = await _context.PuestosDesempenados.FindAsync(id);
            if (puestosDesempenados != null)
            {
                _context.PuestosDesempenados.Remove(puestosDesempenados);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PuestosDesempenadosExists(int id)
        {
            return _context.PuestosDesempenados.Any(e => e.IdPuestoDesempenado == id);
        }
    }
}
