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
    public class PerfilAptitudesController : Controller
    {
        private readonly AppBdContext _context;

        public PerfilAptitudesController(AppBdContext context)
        {
            _context = context;
        }

        // GET: PerfilAptitudes
        public async Task<IActionResult> Index(int? id)
        {
            var lista = listasAptitudesPerfil((int) id);
            foreach (var aptitud in lista)
            {
                aptitud.descripcion = aptitudes(aptitud.IdAptitudes).Descripcion;
              

            }

            return View(lista);
        }

        private List<PerfilAptitudes> listasAptitudesPerfil(int id)
        {
            var listaAptitudes = _context.PerfilAptitudes.FromSql($"EXEC Sp_Cns_ListaAptitudesPerfil @idEmpleado={id}").ToList();
            return listaAptitudes;
        }


        private Aptitudes aptitudes(int id)
        {
            var aptitudes = _context.Aptitudes
                 .FirstOrDefault(m => m.IdAptitud == id);
            return aptitudes;
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

            var lista = listasAptitudesEscoger(perfilAptitudes.IdEmpleado);

            ViewBag.Aptitudes = lista;
            return View(perfilAptitudes);
        }

        private List<Aptitudes> listasAptitudesEscoger(int id)
        {
            var listaAptitudes = _context.Aptitudes.FromSql($"EXEC Sp_Cns_ListaPerfilAptitudesEscoger @idEmpleado={id}").ToList();
            return listaAptitudes;
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

           
          

            var idIdEmpleado = ObtenerIdEmpleadoAutenticado();
             var perfilAptitudes = obtenrPerfilAptitud(idIdEmpleado, (int)id);
          
            
     
            

            if (perfilAptitudes != null)
            {
                _context.PerfilAptitudes.Remove(perfilAptitudes);
            }
            else
            {
                return NotFound();
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("MiPerfil", "PerfilProfesional");

           
        }

     

        private PerfilAptitudes obtenrPerfilAptitud(int idEmpleado, int idAptitudes)
        {
            
             var lista = _context.PerfilAptitudes.FromSql($"EXEC Sp_Cns_obtenerPerfilAptitud @idEmpleado={idEmpleado},@idAptitud={idAptitudes}").ToList();
            var perfilA = lista.FirstOrDefault();

            
                return perfilA;
           
            
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
        private bool PerfilAptitudesExists(int id)
        {
            return _context.PerfilAptitudes.Any(e => e.IdAptitudes == id);
        }
    }
}
