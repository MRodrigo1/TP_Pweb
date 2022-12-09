using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TP_Pweb.Data;
using TP_Pweb.Models;

namespace TP_Pweb.Controllers
{
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.reservas.Include(r => r.EstadoEntrega).Include(r => r.EstadoRecolha).Include(r => r.Utilizador).Include(r => r.Veiculo);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reservas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.reservas == null)
            {
                return NotFound();
            }

            var reserva = await _context.reservas
                .Include(r => r.EstadoEntrega)
                .Include(r => r.EstadoRecolha)
                .Include(r => r.Utilizador)
                .Include(r => r.Veiculo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // GET: Reservas/Create
        public IActionResult Create()
        {
            ViewData["EstadoEntregaId"] = new SelectList(_context.Set<Estado>(), "Id", "Id");
            ViewData["EstadoRecolhaId"] = new SelectList(_context.Set<Estado>(), "Id", "Id");
            ViewData["UtilizadorId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["VeiculoId"] = new SelectList(_context.veiculos, "Id", "Id");
            return View();
        }

        // POST: Reservas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Concluida,EstadoEntregaId,EstadoRecolhaId,UtilizadorId,VeiculoId")] Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoEntregaId"] = new SelectList(_context.Set<Estado>(), "Id", "Id", reserva.EstadoEntregaId);
            ViewData["EstadoRecolhaId"] = new SelectList(_context.Set<Estado>(), "Id", "Id", reserva.EstadoRecolhaId);
            ViewData["UtilizadorId"] = new SelectList(_context.Users, "Id", "Id", reserva.UtilizadorId);
            ViewData["VeiculoId"] = new SelectList(_context.veiculos, "Id", "Id", reserva.VeiculoId);
            return View(reserva);
        }

        // GET: Reservas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.reservas == null)
            {
                return NotFound();
            }

            var reserva = await _context.reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }
            ViewData["EstadoEntregaId"] = new SelectList(_context.Set<Estado>(), "Id", "Id", reserva.EstadoEntregaId);
            ViewData["EstadoRecolhaId"] = new SelectList(_context.Set<Estado>(), "Id", "Id", reserva.EstadoRecolhaId);
            ViewData["UtilizadorId"] = new SelectList(_context.Users, "Id", "Id", reserva.UtilizadorId);
            ViewData["VeiculoId"] = new SelectList(_context.veiculos, "Id", "Id", reserva.VeiculoId);
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Concluida,EstadoEntregaId,EstadoRecolhaId,UtilizadorId,VeiculoId")] Reserva reserva)
        {
            if (id != reserva.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reserva);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaExists(reserva.Id))
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
            ViewData["EstadoEntregaId"] = new SelectList(_context.Set<Estado>(), "Id", "Id", reserva.EstadoEntregaId);
            ViewData["EstadoRecolhaId"] = new SelectList(_context.Set<Estado>(), "Id", "Id", reserva.EstadoRecolhaId);
            ViewData["UtilizadorId"] = new SelectList(_context.Users, "Id", "Id", reserva.UtilizadorId);
            ViewData["VeiculoId"] = new SelectList(_context.veiculos, "Id", "Id", reserva.VeiculoId);
            return View(reserva);
        }

        // GET: Reservas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.reservas == null)
            {
                return NotFound();
            }

            var reserva = await _context.reservas
                .Include(r => r.EstadoEntrega)
                .Include(r => r.EstadoRecolha)
                .Include(r => r.Utilizador)
                .Include(r => r.Veiculo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.reservas == null)
            {
                return Problem("Entity set 'ApplicationDbContext.reservas'  is null.");
            }
            var reserva = await _context.reservas.FindAsync(id);
            if (reserva != null)
            {
                _context.reservas.Remove(reserva);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservaExists(int id)
        {
          return _context.reservas.Any(e => e.Id == id);
        }
    }
}
