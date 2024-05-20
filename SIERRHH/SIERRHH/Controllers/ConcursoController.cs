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
    public class ConcursoController : Controller
    {
        private readonly AppBdContext _context;

        public ConcursoController(AppBdContext context)
        {
            _context = context;
        }

        // GET: Concurso
        public async Task<IActionResult> Index(int? id)
        {
            var listaConcursante = listaConcursantes((int)id);
            foreach (var concursante in listaConcursante)
            {
                concursante.nombreEmpleado = nombreEmpleado(concursante.IdEmpleado);
            }
            return View(listaConcursante);
        }
        private List<Concurso> listaConcursantes(int id)
        {
            var listaConcursante = _context.Concursos.FromSql($"EXEC Sp_Cns_ListaConcursante @IdPuesto={id}").ToList();

            return listaConcursante;
        }

       

        // GET: Concurso/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concurso = await _context.Concursos
                .FirstOrDefaultAsync(m => m.IdEmpleado == id);
            if (concurso == null)
            {
                return NotFound();
            }

            return View(concurso);
        }

        // GET: Concurso/Create
        public IActionResult Create(int? id)
        {
            int idEmpleado = ObtenerIdEmpleadoAutenticado();
            Concurso concursante = new Concurso();
            var concursoVerificar = verificarParticipacion(idEmpleado,(int)id);

            if (concursoVerificar != 0)
            {
                return RedirectToAction("YaConcursado");
            }

           
            concursante.nombreEmpleado = nombreEmpleado(idEmpleado);
            concursante.nombrePuesto = nombrePuesto((int)id);
            concursante.IdEmpleado = idEmpleado;
            concursante.IdPuesto = (int)id;

            return View(concursante);
        }

        public IActionResult YaConcursado()
        {
            return View();
        }

        private string nombreEmpleado(int idEmpleado)
        {
            string nombre = "";

            var perfil = _context.PerfilProfesional.Find(idEmpleado);

            nombre = perfil.Nombre + " " + perfil.Apellido;

            return nombre;
        }

        private string nombrePuesto(int idPuesto)
        {
            string nombre = "";

            var puesto = _context.PuestosVacantes.Find(idPuesto);

            nombre = puesto.Descripcion;

            return nombre;
        }


        private int verificarParticipacion(int idEmpleado, int idPuesto)
        {
            var concursoParticipa = _context.Concursos.FromSql($"EXEC Sp_Cns_verificarParticipacion @idEmpleado={idEmpleado},@idPuesto={idPuesto}").ToList();

            var concursoVerifica = concursoParticipa.Count();
            return concursoVerifica;
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

        // POST: Concurso/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdConcurso,IdPuesto,IdEmpleado,ExperienciaPuesto,Puntaje")] Concurso concurso)
        {
            if (ModelState.IsValid)
            {
                //controles
                int idSector = obtenerSectorPuesto(concurso.IdPuesto);
                int puntaje = 0;
                //Verificar si el empleado tiene un titulo o certificado que sea del sector del puesto 

                int verificarTitulos = 0;
                int verificarCertificaciones = 0;

                var listaTitulos = verificarSectorTitulos(concurso.IdEmpleado, idSector);
                var listaCerficaciones = verificarSectorCertificacion(concurso.IdEmpleado, idSector);

                verificarTitulos = listaTitulos.Count();
                verificarCertificaciones = listaCerficaciones.Count();

                if ((verificarCertificaciones > 0) || (verificarTitulos > 0))
                {
                    if (verificarTitulos > 0)
                    {
                        var listaTitulo = listasTitulos(concurso.IdEmpleado);
                        foreach (var titulos in listaTitulo)
                        {
                            puntaje += obtnerPuntosGrado(titulos.IdGrado);
                        }
                    }

                    // verificar que las aptitudes sean iguales en puesto aptitudes y perfil aptitudes
                    var listaAptitudes = listasAptitudesCumplidas(concurso.IdEmpleado,concurso.IdPuesto);
                    foreach (var aptitudes in listaAptitudes)
                    {
                        puntaje += aptitudes.Puntaje;
                    }

                    concurso.Puntaje = puntaje;
                    _context.Concursos.Add(concurso);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("RegistroCorrecto");
                }
                else
                {
                    return RedirectToAction("RequisistosInvalidos");
                }


                
            }
            return View(concurso);
        }

        public IActionResult RequisistosInvalidos()
        {
            return View();
        }

        public IActionResult RegistroCorrecto()
        {
            return View();
        }

        private List<Aptitudes> listasAptitudesCumplidas(int idEmpleado, int idPuesto)
        {
            var listaAptitudesCumplidas = _context.Aptitudes.FromSql($"EXEC Sp_Cns_ListaAptitudesCumplidas @idEmpleado={idEmpleado},@idPuesto={idPuesto}").ToList();
            return listaAptitudesCumplidas;
        }

        private List<Titulos> listasTitulos(int idEmpleado)
        {
            var listaTitulos = _context.Titulos.FromSql($"EXEC Sp_Cns_ListaTitulos @idEmpleado={idEmpleado}").ToList();
            return listaTitulos;
        }

        private int obtnerPuntosGrado(int idGrado)
        {
            int puntos = 0;

            var grado =_context.Grado.Find(idGrado);

            puntos = grado.Puntaje;

            return puntos;
        }

        private List<Titulos> verificarSectorTitulos(int idEmpleado, int idSector)
        {
            var titulos = _context.Titulos.FromSql($"EXEC Sp_Cns_verificarSectorTitulos @idEmpleado={idEmpleado},@idSector={idSector}").ToList();

            
            return titulos;
        }

        private List<Certificacion> verificarSectorCertificacion(int idEmpleado, int idSector)
        {
            var certificacion = _context.Certificacion.FromSql($"EXEC Sp_Cns_verificarSectorCertificaciones @idEmpleado={idEmpleado},@idSector={idSector}").ToList();


            return certificacion;
        }

        private int obtenerSectorPuesto(int idPuesto)
        {
            int idSector = 0;

            var puestos = _context.PuestosVacantes.Find(idPuesto);

            idSector = puestos.IdSector;
            return idSector;
        }

        // GET: Concurso/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concurso = await _context.Concursos.FindAsync(id);
            if (concurso == null)
            {
                return NotFound();
            }
            return View(concurso);
        }

        // POST: Concurso/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdConcurso,IdPuesto,IdEmpleado,ExperienciaPuesto,Puntaje")] Concurso concurso)
        {
            if (id != concurso.IdEmpleado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(concurso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConcursoExists(concurso.IdEmpleado))
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
            return View(concurso);
        }

        // GET: Concurso/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concurso = await _context.Concursos
                .FirstOrDefaultAsync(m => m.IdEmpleado == id);
            if (concurso == null)
            {
                return NotFound();
            }

            return View(concurso);
        }

        // POST: Concurso/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var concurso = await _context.Concursos.FindAsync(id);
            if (concurso != null)
            {
                _context.Concursos.Remove(concurso);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConcursoExists(int id)
        {
            return _context.Concursos.Any(e => e.IdEmpleado == id);
        }
    }
}
