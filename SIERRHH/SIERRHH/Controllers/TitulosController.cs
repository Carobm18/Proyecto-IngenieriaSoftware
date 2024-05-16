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
    public class TitulosController : Controller
    {
        private readonly AppBdContext _context;

        public TitulosController(AppBdContext context)
        {
            _context = context;
        }

        // GET: Titulos
        public async Task<IActionResult> Index(int? id)
        {
            var listaTitulos = listasTitulos((int)id);
            foreach (var titulos in listaTitulos)
            {
                titulos.nombreSector = sector(titulos.IdSector).NombreSector;
                titulos.nombreGrado = grado(titulos.IdGrado).Descripcion;

            }

            return View(listaTitulos);
        }



        private List<Titulos> listasTitulos(int id)
        {
            var listaTitulos = _context.Titulos.FromSql($"EXEC Sp_Cns_ListaTitulos @idEmpleado={id}").ToList();
            return listaTitulos;
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
            int idEmpleado = ObtenerIdEmpleadoAutenticado();

            // Crea una nueva instancia de PuestosDesempenados con el IdEmpleado asignado
            var titulos = new Titulos
            {
                IdEmpleado = idEmpleado
            };
            ViewBag.Sectores = _context.Sector.ToList();
            ViewBag.Grados = _context.Grado.ToList();

            return View(titulos);
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
                return RedirectToAction("MiPerfil", "PerfilProfesional");
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
            ViewBag.Sectores = _context.Sector.ToList();
            ViewBag.Grados = _context.Grado.ToList();
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

        private Sector sector(int id)
        {
            var sector = _context.Sector
                 .FirstOrDefault(m => m.IdSector == id);
            return sector;
        }

        private Grado grado(int id)
        {
            var grado = _context.Grado
                 .FirstOrDefault(m => m.IdGrado == id);
            return grado;
        }
    }
}
