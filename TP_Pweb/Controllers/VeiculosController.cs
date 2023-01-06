using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<Utilizador> _userManager;

        public VeiculosController(ApplicationDbContext context, UserManager<Utilizador> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Veiculos
        [Authorize(Roles = "Funcionario,Gestor")]
        public async Task<IActionResult> Index()
        {
            var veiculos = await _context.veiculos.Include(v => v.categoria).Include(v => v.empresa).ToListAsync();
            var user = await _userManager.GetUserAsync(User);
            var veiculosSel = new List<Veiculo>();

            foreach (var veiculo in veiculos) {
                if (veiculo.EmpresaId == user.EmpresaId)
                    veiculosSel.Add(veiculo);
            }

            return View(veiculosSel);
        }
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> AllVeiculos()
        {
            var veiculos = await _context.veiculos.Include(v => v.categoria).Include(v => v.empresa).ToListAsync();
            return View(veiculos);
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

        public async Task<IActionResult> RealizaReserva(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RealizaReserva(int id,[Bind("UtilizadorId,VeiculoId,DataInicialPesquisa,DataFinalPesquisa")] Veiculo veiculo)
        {
            veiculo = (Veiculo)_context.veiculos.Where(v => v.Id == id);
            return View(veiculo);
        }

        // GET: Veiculos/Create
        [Authorize(Roles = "Funcionario,Gestor")]
        public async Task<IActionResult> Create()
        {

            ViewData["CategoriaId"] = new SelectList(_context.categorias, "Id", "Nome");
            var user = await _userManager.GetUserAsync(User);
            var empresa = await _context.Empresa.Where(e => e.Id == user.EmpresaId).FirstOrDefaultAsync();
            if (empresa != null)
            {
                //ViewBag.EmpresaNome = empresa.Nome;
                ViewData["EmpresaNome"] = new SelectList(_context.Empresa.Where(e =>e.Nome == empresa.Nome), "Id", "Nome");
            }

            return View();
        }

        // POST: Veiculos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Funcionario,Gestor")]
        public async Task<IActionResult> Create([Bind("Id,FotoDisplay,Modelo,Localizacao,CustoDia,nrKm,EmpresaId,CategoriaId")] Veiculo veiculo,IFormFile FotoVeiculo)
        {
            //veiculo.empresa = _context.Empresa.Where(empresaid =>Empresa)
            //ViewData["CategoriaId"] = new SelectList(_context.categorias, "Id", "Nome");
            ViewBag.CategoriaId = new SelectList(_context.categorias, "Id", "Nome");

            var user = await _userManager.GetUserAsync(User);
            var empresa = await _context.Empresa.Where(e => e.Id == user.EmpresaId).FirstOrDefaultAsync();
            if(empresa != null) {
                ViewBag.EmpresaNome = empresa.Nome;
            }
            veiculo.EmpresaId = user.EmpresaId;
            //TODO Remove empresa e categoria, verificação
            ModelState.Remove(nameof(veiculo.empresa));
            ModelState.Remove(nameof(veiculo.categoria));
            veiculo.ativo = true;
            //TODO MENSAGEM DE ERRO

            if (FotoVeiculo == null)
                ModelState.Remove(nameof(FotoVeiculo));

            if (ModelState.IsValid && veiculo.EmpresaId!=null)
                {
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/VeiculosPhotos/" + veiculo.Id)))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/VeiculosPhotos/" + veiculo.Id));
                }
                if (FotoVeiculo != null)
                {
                    if (FotoVeiculo.Length <= (200 * 1024) && isValidFileType(FotoVeiculo.FileName))
                    {
                        using (var dataStream = new MemoryStream())
                        {
                            await FotoVeiculo.CopyToAsync(dataStream);
                            veiculo.FotoDisplay = dataStream.ToArray();
                        }
                    }
                    else
                    {
                        TempData["Error"] = String.Format("Imagem demasiado Grande.");
                        return View(veiculo);
                    }
                }
                _context.Add(veiculo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(veiculo);
        }

        // GET: Veiculos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["CategoriaId"] = new SelectList(_context.categorias, "Id", "Nome");
            ViewData["EmpresaId"] = new SelectList(_context.Set<Empresa>(), "Id", "Nome");

            if (id == null || _context.veiculos == null)
            {
                return NotFound();
            }

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/VeiculosPhotos");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string VeiculoPath = Path.Combine(Directory.GetCurrentDirectory(),
                   "wwwroot\\VeiculosPhotos\\" + id.ToString());

            if (!Directory.Exists(VeiculoPath))
                Directory.CreateDirectory(VeiculoPath);

            var files = from file in Directory.EnumerateFiles(VeiculoPath)
                        select string.Format("/VeiculosPhotos/{0}/{1}", id, Path.GetFileName(file));

            ViewData["ficheiros"] = files;
            ViewBag.ficheiros = files;

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
        public async Task<IActionResult> Edit(int id, [Bind("Id,FotoDisplay,Modelo,Localizacao,CustoDia,nrKm,EmpresaId,CategoriaId")] Veiculo veiculo, IFormFile FotoVeiculo, [FromForm] List<IFormFile> fotosVeiculos)
        {
            ViewData["CategoriaId"] = new SelectList(_context.categorias, "Id", "Nome");
            ViewData["EmpresaId"] = new SelectList(_context.Set<Empresa>(), "Id", "Nome");
            if (id != veiculo.Id)
            {
                return NotFound();
            }
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/VeiculosPhotos");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string VeiculoPath = Path.Combine(Directory.GetCurrentDirectory(),
                   "wwwroot\\VeiculosPhotos\\" + id.ToString());

            if (!Directory.Exists(VeiculoPath))
                Directory.CreateDirectory(VeiculoPath);

            var files = from file in Directory.EnumerateFiles(VeiculoPath)
                        select string.Format("/VeiculosPhotos/{0}/{1}", id, Path.GetFileName(file));

            ViewData["ficheiros"] = files;

            if (FotoVeiculo == null) {
                ModelState.Remove(nameof(FotoVeiculo));//TODO VERIFICAR O TAMANHO DA IMAGEM
            }

            ModelState.Remove(nameof(veiculo.empresa));
            ModelState.Remove(nameof(veiculo.categoria));

            if (ModelState.IsValid)
            {
                
                if (FotoVeiculo != null)
                {
                    if (FotoVeiculo.Length <= (200 * 1024) && isValidFileType(FotoVeiculo.FileName))
                    {
                        using (var dataStream = new MemoryStream())
                        {
                            await FotoVeiculo.CopyToAsync(dataStream);
                            veiculo.FotoDisplay = dataStream.ToArray();

                        }
                    }
                    else
                    {
                        TempData["Error"] = String.Format("Imagem demasiado Grande.");
                        return RedirectToAction(); //TODO VERIFICAR O TAMANHO DA IMAGEM
                    }
                }
                try
                {
                    foreach (var formFile in fotosVeiculos)
                    {
                        if (formFile.Length > 0)
                        {
                            var filePath = Path.Combine(VeiculoPath, Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName));

                            while (System.IO.File.Exists(filePath))
                            {
                                filePath = Path.Combine(VeiculoPath, Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName));
                            }

                            using (var stream = System.IO.File.Create(filePath))
                            {
                                await formFile.CopyToAsync(stream);
                            }
                        }
                    }
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
            
            var reservas = await _context.reservas.ToListAsync();

            Boolean isReservado = true;
            foreach (var reserva in reservas)
            {
                if (reserva.VeiculoId == id)
                    isReservado = false;
            }

            if (veiculo != null && isReservado)
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
                pesquisaVeiculo.DataInicialPesquisa = DataInicialPesquisa;
                pesquisaVeiculo.DataFinalPesquisa = DataFinalPesquisa;

            //Verificar data nas reservas
            var reservas = _context.reservas.ToList();

            //foreach (Veiculo v in pesquisaVeiculo.ListaDeVeiculos)
            //{
            //    foreach (Reserva r in reservas)
            //    {
            //        if (v.Id == r.VeiculoId)
            //        {
            //            if (DataInicialPesquisa < r.DataEntrega && DataFinalPesquisa < r.DataEntrega)
            //            {
            //                pesquisaVeiculo.ListaDeVeiculos.Remove(v);
            //            }
            //            else if (DataInicialPesquisa > r.DataRecolha && DataFinalPesquisa > r.DataRecolha)
            //            {
            //                pesquisaVeiculo.ListaDeVeiculos.Remove(v);
            //            }
            //            //ADICIONAR NA LISTA NOVA   
            //        }
            //    }
            //}

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

            //pesquisaVeiculo.LocalizacaoPesquisa = pesquisaVeiculo.LocalizacaoPesquisa;
            //pesquisaVeiculo.CategoriaPesquisa = pesquisaVeiculo.CategoriaPesquisa;
            //Verificar data nas reservas
            var reservas = _context.reservas.ToList();
            foreach (Veiculo v in pesquisaVeiculo.ListaDeVeiculos)
            {
                foreach (Reserva r in reservas)
                {
                    if (v.Id == r.VeiculoId)
                    {
                        if (pesquisaVeiculo.DataInicialPesquisa < r.DataEntrega && pesquisaVeiculo.DataFinalPesquisa < r.DataEntrega)
                        {
                            pesquisaVeiculo.ListaDeVeiculos.Remove(v);
                        }
                        else if (pesquisaVeiculo.DataInicialPesquisa > r.DataRecolha && pesquisaVeiculo.DataFinalPesquisa > r.DataRecolha)
                        {
                            pesquisaVeiculo.ListaDeVeiculos.Remove(v);
                        }
                    }
                }
            }
            

            pesquisaVeiculo.NumResultados = pesquisaVeiculo.ListaDeVeiculos.Count();


            return View(pesquisaVeiculo);
        }
            bool isValidFileType(string fileName)
            {
                if (fileName.EndsWith(".png") || fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg"))
                {
                    return true;
                }
                else
                    return false;
            }

        [Authorize(Roles = "Administrador,Funcionario,Gestor")]
        public async Task<IActionResult> AtivaDesativaVeiculos(int? id)
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
            return View(veiculo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Funcionario,Gestor")]
        public async Task<IActionResult> AtivaDesativaVeiculos(int id, [Bind("Id,FotoDisplay,Modelo,Localizacao,CustoDia,nrKm,EmpresaId,CategoriaId,ativo")] Veiculo veiculo)
        {
            if (id == null || _context.veiculos == null)
            {
                return NotFound();
            }
            var vei = await _context.veiculos.FirstOrDefaultAsync(m => m.Id == id);

            if (vei == null)
            {
                return NotFound();
            }


            var veiculoselecionado = await _context.veiculos.Where(v => v.Id == id).FirstAsync();

            if (veiculoselecionado == null)
                return NotFound();

            veiculoselecionado.ativo = veiculo.ativo;

            _context.Update(veiculoselecionado);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> deleteImage(int id, string image)
        {
            if (id == null || _context.veiculos == null)
                return NotFound();
            var veiculo = await _context.veiculos.FirstOrDefaultAsync(v => v.Id == id);

            if (veiculo == null)
                return NotFound();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/" + image);

            System.IO.File.Delete(filePath);
            return RedirectToAction("Edit", new { Id = id });
        }



    }
}
