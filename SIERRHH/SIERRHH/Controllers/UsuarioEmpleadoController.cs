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
            return View();
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
    }
}
