﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<Utilizador> _userManager;
        public ReservasController(ApplicationDbContext context, UserManager<Utilizador> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reservas
        [Authorize(Roles = "Funcionario,Gestor")]
        public async Task<IActionResult> Index()
        {
            //.Include(r => r.estados)
            var reservas = await _context.reservas.Include(r => r.Utilizador).Include(r => r.Veiculo).ToListAsync();
            var user = await _userManager.GetUserAsync(User);
            var veiculo = await _context.veiculos.Where(v => v.EmpresaId == user.EmpresaId).ToListAsync();
            var ListaReservas = new List<Reserva>();
            foreach (var reserva in reservas)
            {
                if (veiculo.Contains(reserva.Veiculo))
                    ListaReservas.Add(reserva);
            }
            return View(ListaReservas);
        }

        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> AsMinhasReservas()
        {
            var reservas = await _context.reservas.Include(r => r.Utilizador).Include(r => r.Veiculo).ToListAsync();
            var user = await _userManager.GetUserAsync(User);
            var ListaReservas = new List<Reserva>();
            foreach (var reserva in reservas)
            {
                if (reserva.UtilizadorId == user.Id)
                    ListaReservas.Add(reserva);
            }
            return View(ListaReservas);
        }

        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> CancelarReservaCli(int id)
        {
            var reserva = _context.reservas.Where(x => x.Id == id).FirstOrDefault();
            if (reserva != null && reserva.state.Equals(Reserva.State.Pendente))
            {
                reserva.state = Reserva.State.Cancelada;
                _context.reservas.Remove(reserva);
                TempData["Error"] = String.Format("Reserva cancelada.");
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(AsMinhasReservas));
        }

        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> EntregarReservaCli(int id) {
            var reserva = _context.reservas.Where(x => x.Id == id).FirstOrDefault();
            if (reserva != null && reserva.state.Equals(Reserva.State.Decorrer))
            {
                reserva.state = Reserva.State.Entregue;
                _context.Update(reserva);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(AsMinhasReservas));
        }
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> ClassificaEmpresa(int id) {
            var reserva = await _context.reservas.Where(x => x.Id == id).FirstAsync();
            return View(reserva);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> ClassificaEmpresa(int id, int classifica) {
            var reserva = await _context.reservas.Where(x => x.Id == id).FirstAsync();
            var veiculo = await _context.veiculos.Where(v => v.Id == reserva.VeiculoId).FirstAsync();
            if (reserva != null && reserva.state.Equals(Reserva.State.Concluida) && reserva.classificada == false)
            {
                var empresa = await _context.Empresa.Where(e => e.Id == veiculo.EmpresaId).FirstAsync();
                var aux = empresa.avaliacao;
                aux = (aux + classifica) / 2;
                empresa.avaliacao = aux;
                reserva.classificada = true;
                _context.Update(reserva);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(AsMinhasReservas));
        }


        // GET: Reservas/Details/5
        [Authorize(Roles = "Cliente,Funcionario,Gestor")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.reservas == null)
            {
                return NotFound();
            }

            var reserva = await _context.reservas
                .Include(r => r.Utilizador)
                .Include(r => r.Veiculo)
                .Include(r => r.estados)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            string pathEntrega = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/DanosPhotosEntrega");
            if (!Directory.Exists(pathEntrega))
                Directory.CreateDirectory(pathEntrega);

            string DanosPathEntrega = Path.Combine(Directory.GetCurrentDirectory(),
                   "wwwroot\\DanosPhotosEntrega\\" + id.ToString());

            if (!Directory.Exists(DanosPathEntrega))
                Directory.CreateDirectory(DanosPathEntrega);

            var filesEntrega = from file in Directory.EnumerateFiles(DanosPathEntrega)
                        select string.Format("/DanosPhotosEntrega/{0}/{1}", id, Path.GetFileName(file));

            ViewData["ficheirosEntrega"] = filesEntrega;
            ViewBag.ficheirosEntrega = filesEntrega;

            string pathRecolha = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/DanosPhotosRecolha");
            if (!Directory.Exists(pathRecolha))
                Directory.CreateDirectory(pathRecolha);

            string DanosPathRecolha = Path.Combine(Directory.GetCurrentDirectory(),
                   "wwwroot\\DanosPhotosRecolha\\" + id.ToString());

            if (!Directory.Exists(DanosPathRecolha))
                Directory.CreateDirectory(DanosPathRecolha);

            var filesRecolha = from file in Directory.EnumerateFiles(DanosPathRecolha)
                        select string.Format("/DanosPhotosRecolha/{0}/{1}", id, Path.GetFileName(file));

            ViewData["ficheirosRecolha"] = filesRecolha;
            ViewBag.ficheirosRecolha = filesRecolha;

            //Nome dos funcionarios/Getores que aceiaram
            if (reserva.state.Equals(Reserva.State.Entregue) || reserva.state.Equals(Reserva.State.Decorrer) || reserva.state.Equals(Reserva.State.Concluida)) {
            var fEntrega = await _context.estados.Where(e => e.ReservaId == id && e.state.Equals(Estado.State.Entrega)).FirstAsync();
            var funcionarioEntrega = await _context.Users.Where(u => u.Id == fEntrega.FuncionarioId).FirstAsync();
            if(funcionarioEntrega!=null)
                ViewBag.funcionarioEntrega = funcionarioEntrega.PrimeiroNome + " " + funcionarioEntrega.UltimoNome;
            }
            if (reserva.state.Equals(Reserva.State.Concluida))
            {
                var fRecolha = await _context.estados.Where(e => e.ReservaId == id && e.state.Equals(Estado.State.Recolha)).FirstAsync();
                var funcionarioRecolha = await _context.Users.Where(u => u.Id == fRecolha.FuncionarioId).FirstAsync();
                if (funcionarioRecolha != null)
                    ViewBag.funcionarioRecolha = funcionarioRecolha.PrimeiroNome + " " + funcionarioRecolha.UltimoNome;
            }
            if (reserva.state.Equals(Reserva.State.Cancelada)) {
                var fcancelada = await _context.estados.Where(e => e.ReservaId == id && e.state.Equals(Estado.State.Entrega)).FirstAsync();
                var funcionarioCancelamento = await _context.Users.Where(u => u.Id == fcancelada.FuncionarioId).FirstAsync();
                ViewBag.funcionarioCancelamento = funcionarioCancelamento.PrimeiroNome + " " + funcionarioCancelamento.UltimoNome;
            }
            return View(reserva);
        }

        // GET: Reservas/Create
        public IActionResult Create()
        {
            ViewData["UtilizadorId"] = new SelectList(_context.Users, "Id", "PrimeiroNome");
            ViewData["VeiculoId"] = new SelectList(_context.veiculos, "Id", "Modelo");
            return View();
        }

        // POST: Reservas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,state,UtilizadorId,VeiculoId")] Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UtilizadorId"] = new SelectList(_context.Users, "Id", "PrimeiroNome", reserva.UtilizadorId);
            ViewData["VeiculoId"] = new SelectList(_context.veiculos, "Id", "PrimeiroNome", reserva.VeiculoId);
            return View(reserva);
        }

        public IActionResult CreateFromDetails() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> CreateFromDetails([Bind("Id,state,DataRecolha,DataEntrega,UtilizadorId,VeiculoId")] Reserva reserva, int idcar,DateTime di, DateTime df,int preco) {
            //ViewData["AccomodationId"] = new SelectList(_context.Accomodations, "AccomodationId", "Description", booking.AccomodationId);
            //var customer = _context.Customers.Where(x => x.ApplicationUser.Id == applicationUserId).First();

            ModelState.Remove(nameof(reserva.Veiculo));
            ModelState.Remove(nameof(reserva.Utilizador));
            ModelState.Remove(nameof(reserva.UtilizadorId));
            ModelState.Remove(nameof(reserva.estados));
            var user = await _userManager.GetUserAsync(User);
            reserva.DataEntrega = di;
            reserva.DataRecolha = df;
            reserva.VeiculoId = idcar;
            reserva.UtilizadorId = user.Id;
            reserva.state = Reserva.State.Pendente;
            reserva.classificada = false;
            var veiculo = await _context.veiculos.Where(v => v.Id == idcar).FirstAsync();

            reserva.preco = preco;
            // verifica se a data é válida
            //TODO Melhorar função IsValidDate(reserva)

            if (ModelState.IsValid) {
                _context.Add(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction("AsMinhasReservas", "Reservas");
            }
            return View();
        }
        [Authorize(Roles = "Funcionario,Gestor")]
        public async Task<IActionResult> ProcessaEstado(int id)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Funcionario,Gestor")]
        public async Task<IActionResult> ProcessaEstado(int id, [Bind("NrKilometros,danos,observacoes")] Estado estado, [FromForm] List<IFormFile> fotosdanos)
        {
            if (id == null || _context.reservas == null)
            {
                return NotFound();
            }

            var reserva = await _context.reservas.Where(r => r.Id == id).FirstAsync();
            if (reserva.state.Equals(Reserva.State.Pendente))
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/DanosPhotosEntrega");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string danospath = Path.Combine(Directory.GetCurrentDirectory(),
                       "wwwroot\\DanosPhotosEntrega\\" + id.ToString());

                if (!Directory.Exists(danospath))
                    Directory.CreateDirectory(danospath);

                var files = from file in Directory.EnumerateFiles(danospath)
                            select string.Format("/DanosPhotosEntrega/{0}/{1}", id, Path.GetFileName(file));

                ViewData["ficheiros"] = files;
                ViewBag.ficheiros = files;

                foreach (var formFile in fotosdanos)
                {
                    if (formFile.Length > 0)
                    {
                        var filePath = Path.Combine(danospath, Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName));

                        while (System.IO.File.Exists(filePath))
                        {
                            filePath = Path.Combine(danospath, Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName));
                        }

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }
                }
                await ConfirmarReserva(id, estado.danos, estado.observacoes, estado.NrKilometros);
            }
            else if (reserva.state.Equals(Reserva.State.Entregue))
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/DanosPhotosRecolha");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string danospath = Path.Combine(Directory.GetCurrentDirectory(),
                       "wwwroot\\DanosPhotosRecolha\\" + id.ToString());

                if (!Directory.Exists(danospath))
                    Directory.CreateDirectory(danospath);

                var files = from file in Directory.EnumerateFiles(danospath)
                            select string.Format("/DanosPhotosRecolha/{0}/{1}", id, Path.GetFileName(file));

                ViewData["ficheiros"] = files;
                ViewBag.ficheiros = files;

                foreach (var formFile in fotosdanos)
                {
                    if (formFile.Length > 0)
                    {
                        var filePath = Path.Combine(danospath, Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName));

                        while (System.IO.File.Exists(filePath))
                        {
                            filePath = Path.Combine(danospath, Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName));
                        }

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }
                }
                await EncerraReserva(id, estado.danos, estado.observacoes, estado.NrKilometros);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool IsValidDate(Reserva booking) {

            var reservas = _context.reservas.Where(r => r.VeiculoId == booking.VeiculoId);
            foreach (Reserva r in reservas)
            {
                if (booking.DataEntrega < r.DataEntrega && booking.DataRecolha < r.DataEntrega)
                {
                    return false;
                }
                else if (booking.DataEntrega > r.DataRecolha && booking.DataRecolha > r.DataRecolha)
                {
                    return false;
                }
            }
            return true;
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
            ViewData["UtilizadorId"] = new SelectList(_context.Users, "Id", "Id", reserva.UtilizadorId);
            ViewData["VeiculoId"] = new SelectList(_context.veiculos, "Id", "Id", reserva.VeiculoId);
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,state,UtilizadorId,VeiculoId")] Reserva reserva)
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
            ViewData["UtilizadorId"] = new SelectList(_context.Users, "Id", "Id", reserva.UtilizadorId);
            ViewData["VeiculoId"] = new SelectList(_context.veiculos, "Id", "Id", reserva.VeiculoId);
            return View(reserva);
        }

        [Authorize(Roles = "Funcionario,Gestor")]
        public async Task<IActionResult> ConfirmarReserva(int id,bool danos,string observacoes,int nrkm) {

            var reserva = _context.reservas.Where(x => x.Id == id).FirstOrDefault();
            var veiculo = _context.veiculos.Where(x => x.Id == reserva.VeiculoId).FirstOrDefault();
            var func = await _userManager.GetUserAsync(User);
            if (reserva == null)
                return NotFound();
            var estado = new Estado()
            {
                state = Estado.State.Entrega,
                NrKilometros = nrkm,
                danos = danos,
                observacoes = observacoes,
                FuncionarioId = func.Id,
                ReservaId = reserva.Id
            };
            if (!reserva.state.Equals(Reserva.State.Pendente)) {
                TempData["Error"] = String.Format("Erro.");
                return RedirectToAction(nameof(Index));
            }
            var reservas = await _context.reservas.Where(r => r.VeiculoId == veiculo.Id && !r.state.Equals(Reserva.State.Pendente) && !r.state.Equals(Reserva.State.Cancelada) && !r.state.Equals(Reserva.State.Concluida)).ToListAsync();
            bool Valida = true;
            foreach (var r in reservas) {
                if (reserva.DataEntrega < r.DataEntrega && reserva.DataRecolha < r.DataEntrega ||
                             reserva.DataRecolha > r.DataRecolha && reserva.DataRecolha > r.DataRecolha)
                {
                }
                else
                {
                    Valida = false;
                }
            }


            if (reserva != null && veiculo != null && func != null && Valida)
            {
                List<Estado> states = new List<Estado>();
                states.Add(estado);
                reserva.estados = states;
                reserva.state = Reserva.State.Decorrer;
                _context.Update(reserva);
                await _context.SaveChangesAsync();
            }
            else {
                TempData["Error"] = "Nao foi possivel confimar a reserva.[Conflito datas]";
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Funcionario,Gestor")]
        public async Task<IActionResult> CancelarReserva(int id) {

            var reserva = _context.reservas.Where(x => x.Id == id).FirstOrDefault();
            var func = await _userManager.GetUserAsync(User);
            var estado = new Estado()
            {
                state = Estado.State.Entrega,
                NrKilometros = 0,
                danos = false,
                observacoes = "",
                FuncionarioId = func.Id,
                ReservaId = reserva.Id
            };

            if (reserva != null) {
                if (!reserva.state.Equals(Reserva.State.Pendente))
                {
                    TempData["Error"] = String.Format("Erro.");
                    return RedirectToAction(nameof(Index));
                }

                    List<Estado> states = new List<Estado>();
                    states.Add(estado);
                    reserva.estados = states;
                    reserva.state = Reserva.State.Cancelada;
                    _context.Update(reserva);
                    await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Funcionario,Gestor")]
        public async Task<IActionResult> EncerraReserva(int id,bool danos, string observacoes, int nrkm)
        {
            var reserva = _context.reservas.Where(x => x.Id == id).Include(x => x.estados).FirstOrDefault();
            var veiculo = _context.veiculos.Where(x => x.Id == reserva.VeiculoId).FirstOrDefault();
            var func = await _userManager.GetUserAsync(User);
            if (reserva == null)
                return NotFound();

            if (!reserva.state.Equals(Reserva.State.Entregue))
            {
                TempData["Error"] = String.Format("Erro.");
                return RedirectToAction(nameof(Index));
            }
            var estado = new Estado()
            {
                state = Estado.State.Recolha,
                NrKilometros = nrkm,
                danos = danos,
                observacoes = observacoes,
                FuncionarioId = func.Id,
                ReservaId = reserva.Id
            };
            if (reserva != null && veiculo != null && func != null)
            {
                List<Estado> estadosatuais = new List<Estado>();
                estadosatuais.AddRange(reserva.estados);
                estadosatuais.Add(estado);
                reserva.estados = estadosatuais;
                reserva.state = Reserva.State.Concluida;
                _context.Update(reserva);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


        // GET: Reservas/Delete/5
        [Authorize(Roles = "Ãdministrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.reservas == null)
            {
                return NotFound();
            }

            var reserva = await _context.reservas
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
