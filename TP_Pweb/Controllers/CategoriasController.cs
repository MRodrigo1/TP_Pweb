﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TP_Pweb.Data;
using TP_Pweb.Models;

namespace TP_Pweb.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categorias
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index()
        {
              return View(await _context.categorias.ToListAsync());
        }

        // GET: Categorias/Details/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.categorias == null)
            {
                return NotFound();
            }

            var categoria = await _context.categorias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // GET: Categorias/Create
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // GET: Categorias/Edit/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.categorias == null)
            {
                return NotFound();
            }

            var categoria = await _context.categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // POST: Categorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.Id))
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
            return View(categoria);
        }

        // GET: Categorias/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.categorias == null)
            {
                return NotFound();
            }

            var categoria = await _context.categorias
                .FirstOrDefaultAsync(m => m.Id == id);


            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.categorias == null)
            {
                return Problem("Entity set 'ApplicationDbContext.categorias'  is null.");
            }
            var categoria = await _context.categorias.FindAsync(id);
            var veiculos = await _context.veiculos.Where(v => v.CategoriaId == id).ToListAsync();

            if (veiculos.Count > 0)
            {
                TempData["Error"] = "Categoria nao pode estar incluida em nenhum veiculo";
                return RedirectToAction(nameof(Index));
            }
            if (categoria != null)
            {
                _context.categorias.Remove(categoria);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaExists(int id)
        {
          return _context.categorias.Any(e => e.Id == id);
        }
    }
}
