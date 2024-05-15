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
    public class PerfilProfesionalController : Controller
    {
        private readonly AppBdContext _context;

        public PerfilProfesionalController(AppBdContext context)
        {
            _context = context;
        }

        // GET: PerfilProfesional
        public async Task<IActionResult> Index()
        {
            return View(await _context.PerfilProfesional.ToListAsync());
        }

        // GET: PerfilProfesional/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var perfilProfesional = await _context.PerfilProfesional
                .FirstOrDefaultAsync(m => m.IdEmpleado == id);
            if (perfilProfesional == null)
            {
                return NotFound();
            }

            return View(perfilProfesional);
        }

        // GET: PerfilProfesional/Create
        public IActionResult Create()
        {
           
            return View();
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

        // POST: PerfilProfesional/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEmpleado,Nombre,Apellido,Telefono,Direccion,FechaNacimiento,Descripcion,Foto")] PerfilProfesional perfilProfesional)
        {
            if (ModelState.IsValid)
            {
                int idEmpleado = ObtenerIdEmpleadoAutenticado();

               perfilProfesional.IdEmpleado = idEmpleado;
               
                _context.Add(perfilProfesional);
                await _context.SaveChangesAsync();
                this.cambiarEstadoUsuario(idEmpleado);
                return RedirectToAction(nameof(Index));
            }
            return View(perfilProfesional);
        }

        private  void cambiarEstadoUsuario(int idEmpleado)
        {
            var usuario = _context.UsuarioEmpleado.Find(idEmpleado);
            usuario.Estado = "Activo";
            _context.Update(usuario);
            _context.SaveChangesAsync();
        }

        // GET: PerfilProfesional/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var perfilProfesional = await _context.PerfilProfesional.FindAsync(id);
            if (perfilProfesional == null)
            {
                return NotFound();
            }
            return View(perfilProfesional);
        }

        // POST: PerfilProfesional/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEmpleado,Nombre,Apellido,Telefono,Direcion,FechaNacimiento,Descripcion,Foto")] PerfilProfesional perfilProfesional)
        {
            if (id != perfilProfesional.IdEmpleado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(perfilProfesional);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerfilProfesionalExists(perfilProfesional.IdEmpleado))
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
            return View(perfilProfesional);
        }

        // GET: PerfilProfesional/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var perfilProfesional = await _context.PerfilProfesional
                .FirstOrDefaultAsync(m => m.IdEmpleado == id);
            if (perfilProfesional == null)
            {
                return NotFound();
            }

            return View(perfilProfesional);
        }

        // POST: PerfilProfesional/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var perfilProfesional = await _context.PerfilProfesional.FindAsync(id);
            if (perfilProfesional != null)
            {
                _context.PerfilProfesional.Remove(perfilProfesional);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PerfilProfesionalExists(int id)
        {
            return _context.PerfilProfesional.Any(e => e.IdEmpleado == id);
        }

        //Programación mi perfil
        public IActionResult MiPerfil()
        {
            int idEmpleado = ObtenerIdEmpleadoAutenticado();

            var perfil = _context.PerfilProfesional.Find(idEmpleado);
            if (perfil != null)
            {
                var aptitudes = listasAptitudes(perfil.IdEmpleado);

                var titulos = listasTitulos(perfil.IdEmpleado);

                var certificaciones = listasCertificacion(perfil.IdEmpleado);

                var capacitaciones = listasCapacitacion(perfil.IdEmpleado);
                var puestosDesmp = listasPuestosDesemp(perfil.IdEmpleado);

                perfil.listaAptitudes = aptitudes;
                perfil.listaTitulos = titulos;
                perfil.listaCertificacion = certificaciones;
                perfil.listaCapacitaciones= capacitaciones;
                perfil.listaPuestosDesemp = puestosDesmp;
            }

            return View(perfil);
        }

        private List<Aptitudes> listasAptitudes(int id)
        {
            var listaAptitudes = _context.Aptitudes.FromSql($"EXEC Sp_Cns_ListaAptitudesPerfil @idEmpleado={id}").ToList();
            return listaAptitudes;
        }

        private List<Titulos> listasTitulos(int id)
        {
            var listaTitulos = _context.Titulos.FromSql($"EXEC Sp_Cns_ListaTitulos @idEmpleado={id}").ToList();
            return listaTitulos;
        }

        private List<Certificacion> listasCertificacion(int id)
        {
            var listaCertificacion = _context.Certificacion.FromSql($"EXEC Sp_Cns_ListaCertificaciones @idEmpleado={id}").ToList();
            return listaCertificacion;
        }

        private List<Capacitacion> listasCapacitacion(int id)
        {
            var listaCapacitacion = _context.Capacitacion.FromSql($"EXEC Sp_Cns_ListaCapacitacion @idEmpleado={id}").ToList();
            return listaCapacitacion;
        }

        private List<PuestosDesempenados> listasPuestosDesemp(int id)
        {
            var listaPuestosDesemp = _context.PuestosDesempenados.FromSql($"EXEC Sp_Cns_ListaPuestosDesemp @idEmpleado={id}").ToList();
            return listaPuestosDesemp;
        }
    }
}
