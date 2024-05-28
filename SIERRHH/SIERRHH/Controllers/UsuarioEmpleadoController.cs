using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIERRHH.Models;
using SIERRHH.Models.constantes;

namespace SIERRHH.Controllers
{
    public class UsuarioEmpleadoController : Controller
    {
        private readonly AppBdContext _context;

        public UsuarioEmpleadoController(AppBdContext context)
        {
            _context = context;
        }

        // GET: UsuarioEmpleadoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.UsuarioEmpleado.ToListAsync());
        }

        // GET: UsuarioEmpleadoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioEmpleado = await _context.UsuarioEmpleado
                .FirstOrDefaultAsync(m => m.IdEmpleado == id);
            if (usuarioEmpleado == null)
            {
                return NotFound();
            }

            return View(usuarioEmpleado);
        }

        // GET: UsuarioEmpleadoes/Create
        public IActionResult Create()
        {
            UsuarioEmpleado usuario = new UsuarioEmpleado();
            usuario.Estado = "Nuevo";
            return View(usuario);
        }

        // POST: UsuarioEmpleadoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEmpleado,Correo,Password,Rol,Estado")] UsuarioEmpleado usuarioEmpleado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuarioEmpleado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuarioEmpleado);
        }

        // GET: UsuarioEmpleadoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioEmpleado = await _context.UsuarioEmpleado.FindAsync(id);
            if (usuarioEmpleado == null)
            {
                return NotFound();
            }
            return View(usuarioEmpleado);
        }

        // POST: UsuarioEmpleadoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEmpleado,Correo,Password,Rol,Estado")] UsuarioEmpleado usuarioEmpleado)
        {
            if (id != usuarioEmpleado.IdEmpleado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuarioEmpleado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioEmpleadoExists(usuarioEmpleado.IdEmpleado))
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
            return View(usuarioEmpleado);
        }

        // GET: UsuarioEmpleadoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioEmpleado = await _context.UsuarioEmpleado
                .FirstOrDefaultAsync(m => m.IdEmpleado == id);
            if (usuarioEmpleado == null)
            {
                return NotFound();
            }

            return View(usuarioEmpleado);
        }

        // POST: UsuarioEmpleadoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuarioEmpleado = await _context.UsuarioEmpleado.FindAsync(id);
            if (usuarioEmpleado != null)
            {
                _context.UsuarioEmpleado.Remove(usuarioEmpleado);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioEmpleadoExists(int id)
        {
            return _context.UsuarioEmpleado.Any(e => e.IdEmpleado == id);
        }

        // Parte del login

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Login([Bind] UsuarioEmpleado user)
        {

            //se utiliza el metodo para validar  los datos del usuario
            var temp = this.ValidarUsuario(user);


            //Se valida si hay datos
            if (temp != null)
            {
                //Declaracion de variable
                bool restablecer = false;

                //Verifica si el usuario necesita restablecer
                //restablecer = this.VerificarEst(temp);
                //Si el valor es true, es el primer inico de session, entonces debe restablecer la contraseña
                if (restablecer)
                {
                    //ojo enviamos un parametro al metodo restablecer
                    return RedirectToAction("Create", "PerfilProfesional");
                }
                else
                {
                    //se crea la instancia para la entidad del usuario y el tipo de autenticacion
                    var userClaims = new List<Claim>() { 
                        new Claim(ClaimTypes.Name, temp.Correo),
                        new Claim(ClaimTypes.NameIdentifier, temp.IdEmpleado.ToString()),
                        new Claim(ClaimTypes.Role, temp.Rol),
                        new Claim(CustomClaimTypes.Estado, temp.Estado)
                    };
                    var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
                    var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });

                    //Se realiza la autenticacion dentro del contexto 
                    HttpContext.SignInAsync(userPrincipal);

                    bool nuevo = false;

                    nuevo = this.VerificarEstado(temp);
                    if (nuevo)
                    {
                        return RedirectToAction("Create", "PerfilProfesional");
                    }
                    
                    //Se ubica ala usuario en la pagina inicio
                    return RedirectToAction("Index", "Home");
                }
            }

            TempData["Mensaje"] = "Error el correo o contraseña son incorrectos ..";
            return View(user);

        }


        public IActionResult RecuperarUsuario()
        {
            return View();
        }

        private UsuarioEmpleado ValidarUsuario(UsuarioEmpleado temp)
        {
            UsuarioEmpleado autorizado = null;

            //Se busca al usuario en la bD
            var user = _context.UsuarioEmpleado.FirstOrDefault(u => u.Correo == temp.Correo);

            if (user != null)
            {
                if (user.Password.Equals(temp.Password))
                {
                    autorizado = user;
                }
            }
            return autorizado;


        }

        private bool VerificarEstado(UsuarioEmpleado temp)
        {
            bool verificado = false;

            //consultar los datos del usuario
            var user = _context.UsuarioEmpleado.First(u => u.Correo == temp.Correo);

            if (user != null)
            {// si restablecer esta en 0 quiere decir que es la primera vez que inicia sesion y debe cambiar la clave
                if (user.Estado == "Nuevo")
                {
                    verificado = true;
                }
            }

            return verificado;
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }


    }
}
