﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TP_Pweb.Data;
using TP_Pweb.Models;
using TP_Pweb.ViewModels;

namespace TP_Pweb.Controllers
{
    public class VeiculosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VeiculosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Veiculos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.veiculos.Include(v => v.categoria).Include(v => v.empresa);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Veiculos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.veiculos == null)
            {
                return NotFound();
            }

            var veiculo = await _context.veiculos
                .Include(v => v.categoria)
                .Include(v => v.empresa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veiculo == null)
            {
                return NotFound();
            }

            return View(veiculo);
        }

        // GET: Veiculos/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.categorias, "Id", "Nome");
            ViewData["EmpresaId"] = new SelectList(_context.Set<Empresa>(), "Id", "Nome");
            return View();
        }

        // POST: Veiculos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Modelo,Localizacao,custo,nrKm,EmpresaId,CategoriaId")] Veiculo veiculo)
        {
            //veiculo.empresa = _context.Empresa.Where(empresaid =>Empresa)
            ViewData["CategoriaId"] = new SelectList(_context.categorias, "Id", "Nome");
            ViewData["EmpresaId"] = new SelectList(_context.Set<Empresa>(), "Id", "Nome");
            if (ModelState.IsValid)
            {
                _context.Add(veiculo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(veiculo);
        }

        // GET: Veiculos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.veiculos == null)
            {
                return NotFound();
            }

            var veiculo = await _context.veiculos.FindAsync(id);
            if (veiculo == null)
            {
                return NotFound();
            }
            //ViewData["CategoriaId"] = new SelectList(_context.categorias, "Id", "Id", veiculo.CategoriaId);
            //ViewData["EmpresaId"] = new SelectList(_context.Set<Empresa>(), "Id", "Id", veiculo.EmpresaId);
            return View(veiculo);
        }

        // POST: Veiculos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Modelo,Localizacao,custo,nrKm,EmpresaId,CategoriaId")] Veiculo veiculo)
        {
            if (id != veiculo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(veiculo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VeiculoExists(veiculo.Id))
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
            //ViewData["CategoriaId"] = new SelectList(_context.categorias, "Id", "Id", veiculo.CategoriaId);
            //ViewData["EmpresaId"] = new SelectList(_context.Set<Empresa>(), "Id", "Id", veiculo.EmpresaId);
            return View(veiculo);
        }

        // GET: Veiculos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.veiculos == null)
            {
                return NotFound();
            }

            var veiculo = await _context.veiculos
                .Include(v => v.categoria)
                .Include(v => v.empresa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veiculo == null)
            {
                return NotFound();
            }

            return View(veiculo);
        }

        // POST: Veiculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.veiculos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.veiculos'  is null.");
            }
            var veiculo = await _context.veiculos.FindAsync(id);
            if (veiculo != null)
            {
                _context.veiculos.Remove(veiculo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VeiculoExists(int id)
        {
          return _context.veiculos.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Search(string LocalizacaoPesquisa,string CategoriaPesquisa, DateTime DataInicialPesquisa, DateTime DataFinalPesquisa)
        {
            PesquisaVeiculoViewModel pesquisaVeiculo = new PesquisaVeiculoViewModel();
            
                pesquisaVeiculo.ListaDeVeiculos = await _context.veiculos.Include("empresa").Include("categoria").Where(
                        v => v.Localizacao.Contains(LocalizacaoPesquisa) &&
                        v.categoria.Nome.Equals(CategoriaPesquisa)
                        ).ToListAsync();
                pesquisaVeiculo.LocalizacaoPesquisa = LocalizacaoPesquisa;
                pesquisaVeiculo.CategoriaPesquisa = CategoriaPesquisa;
            //Verificar data nas reservas
            var reservas = _context.reservas.ToList();
            foreach (Veiculo v in pesquisaVeiculo.ListaDeVeiculos) {
                foreach (Reserva r in reservas) {
                    if (v.Id == r.VeiculoId)
                    {
                        if (DataInicialPesquisa < r.EstadoEntrega.dataReserva && DataFinalPesquisa < r.EstadoEntrega.dataReserva)
                        {
                            pesquisaVeiculo.ListaDeVeiculos.Remove(v);
                        }
                        else if (DataInicialPesquisa > r.EstadoRecolha.dataReserva && DataFinalPesquisa > r.EstadoRecolha.dataReserva)
                        {
                            pesquisaVeiculo.ListaDeVeiculos.Remove(v);
                        }
                    }
                    }
            }
            
            pesquisaVeiculo.NumResultados = pesquisaVeiculo.ListaDeVeiculos.Count();
            return View(pesquisaVeiculo);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search([Bind("LocalizacaoPesquisa,CategoriaPesquisa,DataInicialPesquisa,DataFinalPesquisa")] PesquisaVeiculoViewModel pesquisaVeiculo) {

            pesquisaVeiculo.ListaDeVeiculos = await _context.veiculos.Include("empresa").Include("categoria").Where(
                    v => v.Localizacao.Contains(pesquisaVeiculo.LocalizacaoPesquisa) &&
                    v.categoria.Equals(pesquisaVeiculo.CategoriaPesquisa)
                    ).ToListAsync();

            pesquisaVeiculo.LocalizacaoPesquisa = pesquisaVeiculo.LocalizacaoPesquisa;
            pesquisaVeiculo.CategoriaPesquisa = pesquisaVeiculo.CategoriaPesquisa;
            //Verificar data nas reservas
            var reservas = _context.reservas.ToList();
            foreach (Veiculo v in pesquisaVeiculo.ListaDeVeiculos)
            {
                foreach (Reserva r in reservas)
                {
                    if (v.Id == r.VeiculoId)
                    {
                        if (pesquisaVeiculo.DataInicialPesquisa < r.EstadoEntrega.dataReserva && pesquisaVeiculo.DataFinalPesquisa < r.EstadoEntrega.dataReserva)
                        {
                            pesquisaVeiculo.ListaDeVeiculos.Remove(v);
                        }
                        else if (pesquisaVeiculo.DataInicialPesquisa > r.EstadoRecolha.dataReserva && pesquisaVeiculo.DataFinalPesquisa > r.EstadoRecolha.dataReserva)
                        {
                            pesquisaVeiculo.ListaDeVeiculos.Remove(v);
                        }
                    }
                }
            }

            pesquisaVeiculo.NumResultados = pesquisaVeiculo.ListaDeVeiculos.Count();


            return View(pesquisaVeiculo);
        }
    }
}
