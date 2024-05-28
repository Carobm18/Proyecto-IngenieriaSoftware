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
        public async Task<IActionResult> Index(int? id)
        {
            var lista = listasAptitudesPuesto((int)id);
            foreach (var aptitud in lista)
            {
                aptitud.descripcion = aptitudes(aptitud.IdAptitudes).Descripcion;


            }
            return View(lista);
        }

        //Ver actitudes en el index
        private List<PuestoAptitudes> listasAptitudesPuesto(int id)
        {
            var listaAptitudes = _context.PuestoAptitudes.FromSql($"EXEC Sp_Cns_ListaPuestoAptitudes @IdPuesto={id}").ToList();
            return listaAptitudes;
        }


        private Aptitudes aptitudes(int id)
        {
            var aptitudes = _context.Aptitudes
                 .FirstOrDefault(m => m.IdAptitud == id);
            return aptitudes;
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

            var lista = listasAptitudesEscoger(puestoAptitudes.IdPuesto);

            ViewBag.Aptitudes = lista;
            return View(puestoAptitudes);
        }
        private List<Aptitudes> listasAptitudesEscoger(int id)
        {
            var listaAptitudes = _context.Aptitudes.FromSql($"EXEC Sp_Cns_ListaPuestolAptitudesEscoger @idPuesto={id}").ToList();
            return listaAptitudes;
        }

        // POST: PuestoAptitudes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAptitudes,IdPuesto")] PuestoAptitudes puestoAptitudes)
        {
            if (puestoAptitudes.IdAptitudes == 0 || puestoAptitudes.IdPuesto == 0)
            {
                return RedirectToAction("Details", "PuestosVacantes", new { id = puestoAptitudes.IdPuesto });
            }
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
        public async Task<IActionResult> Delete(int? id, int? name)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var puestoAptitudes = await _context.PuestoAptitudes
            //    .FirstOrDefaultAsync(m => m.IdAptitudes == id);
            var puestoAptitudes = obtenerPuestoAptitud((int)name, (int)id);
            if (puestoAptitudes == null)
            {
                return NotFound();
            }
            
            if (puestoAptitudes != null)
            {
                _context.PuestoAptitudes.Remove(puestoAptitudes);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "PuestosVacantes", new {id =puestoAptitudes.IdPuesto});
        }

        private PuestoAptitudes obtenerPuestoAptitud(int idPuesto, int idAptitudes)
        {

            var lista = _context.PuestoAptitudes.FromSql($"EXEC Sp_Cns_obtenerPuestoAptitud @idPuesto={idPuesto},@idAptitud={idAptitudes}").ToList();
            var perfilA = lista.FirstOrDefault();


            return perfilA;


        }

 
       

        private bool PuestoAptitudesExists(int id)
        {
            return _context.PuestoAptitudes.Any(e => e.IdAptitudes == id);
        }
    }
}
