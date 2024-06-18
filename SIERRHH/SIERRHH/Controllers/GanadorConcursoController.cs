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
    public class GanadorConcursoController : Controller
    {
        private readonly AppBdContext _context;

        public GanadorConcursoController(AppBdContext context)
        {
            _context = context;
        }

       
      

        // GET: GanadorConcurso/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ganadorConcurso = await _context.GanadorConcursos
                .FirstOrDefaultAsync(m => m.IdPuesto == id);
            if (ganadorConcurso == null)
            {
                return NotFound();
            }

            var puesto = _context.PuestosVacantes.Find(ganadorConcurso.IdPuesto);
            var empleado = _context.PerfilProfesional.Find(ganadorConcurso.IdEmpleado);

            ganadorConcurso.nombrePuesto = puesto.Descripcion;
            ganadorConcurso.nombreEmpleado = empleado.Nombre + " " + empleado.Apellido;


            return View(ganadorConcurso);
        }

        // GET: GanadorConcurso/Create
        public IActionResult Create(int? id, int? name)
        {
            //id = idPuesto name= idEmppleado

            var concursante = obtenerConcursante((int)id, (int)name);

             var concursanteGanador = concursante.First();

            GanadorConcurso ganador = new GanadorConcurso();

            ganador.IdPuesto = concursanteGanador.IdPuesto;
            ganador.IdEmpleado = concursanteGanador.IdEmpleado;
            ganador.Estado = "Activo";
            var puesto = _context.PuestosVacantes.Find(ganador.IdPuesto);
            puesto.Estado = "Concluido";

            _context.PuestosVacantes.Update(puesto);
            _context.GanadorConcursos.Add(ganador);
            _context.SaveChanges();

            var usuarioEmpleado =  _context.UsuarioEmpleado.FirstOrDefault(m => m.IdEmpleado == ganador.IdEmpleado);

            var perfilProfesional = _context.PerfilProfesional.FirstOrDefault(m => m.IdEmpleado == ganador.IdEmpleado);


            var correo = usuarioEmpleado.Correo;
            var nombreCompleto = perfilProfesional.Nombre + " " + perfilProfesional.Apellido;

            EnviarEmail(correo, nombreCompleto, puesto.Descripcion);

            return RedirectToAction("Index", "PuestosVacantes");

            
        }

        private async Task<bool> EnviarEmail(String correo, string nombre, String puesto)
        {
            try
            {
                //variable control 
                bool enviado = false;


                //ser instancia el objeto 
                Email email = new Email();

                //se utiliza el metodo para enviar el email
                email.EnviarCorreoConcurso(correo, nombre, puesto);

                //se indica que se envio 
                enviado = true;

                //enviamos el valor 
                return enviado;
            }
            catch (Exception )
            {
                return false;
            }
        }//cierre enviar email

        private List<Concurso> obtenerConcursante(int idPuesto, int idEmpleado)
        {
            var concursante = _context.Concursos.FromSql($"EXEC Sp_Cns_ObetenerConcursante @idPuesto={idPuesto}, @idEmpleado={idEmpleado}").ToList();
            return concursante;
        }

       


     
        // GET: GanadorConcurso/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ganadorConcurso = await _context.GanadorConcursos
                .FirstOrDefaultAsync(m => m.IdPuesto == id);
            if (ganadorConcurso == null)
            {
                return NotFound();
            }
            var puesto = _context.PuestosVacantes.Find(ganadorConcurso.IdPuesto);
            puesto.Estado = "Activo";

            _context.PuestosVacantes.Update(puesto);
            _context.GanadorConcursos.Remove(ganadorConcurso);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "PuestosVacantes");
        }

       
    }
}
