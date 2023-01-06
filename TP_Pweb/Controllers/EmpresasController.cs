using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PWEB_AulasP_2223.Data;
using TP_Pweb.Data;
using TP_Pweb.Models;

namespace TP_Pweb.Controllers
{
    public class EmpresasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilizador> _userManager;
        private readonly SignInManager<Utilizador> _signInManager;

        public EmpresasController(ApplicationDbContext context, UserManager<Utilizador> userManager, SignInManager<Utilizador> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Empresas
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index()
        {
              return View(await _context.Empresa.ToListAsync());
        }

        // GET: Empresas/Details/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Empresa == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresa.FirstOrDefaultAsync(m => m.Id == id);
            if (empresa == null)
            {
                return NotFound();
            }
           
            var users = await _userManager.Users.ToListAsync();
            List<Utilizador> funcionarios = new List<Utilizador>();
            foreach (var user in users) {
                if (user.EmpresaId == empresa.Id) {
                    funcionarios.Add(user);
                }    
            }
            ViewBag.FuncionariosSize = funcionarios.Count;
            ViewBag.FuncionariosEmpresa = funcionarios;

            return View(empresa);
        }
        [Authorize(Roles = "Administrador")]

        public async Task<IActionResult> AtivarDesativarEmpresa(int? id)
        {
            if (id == null || _context.Empresa == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresa.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }
            return View(empresa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AtivarDesativarEmpresa(int id, [Bind("Id,Nome,avaliacao,ativo")] Empresa empresa)
        {
            if (id == null || _context.Empresa == null)
            {
                return NotFound();
            }
            var emp1 = await _context.Empresa.FirstOrDefaultAsync(m => m.Id == id);

            if (emp1 == null)
            {
                return NotFound();
            }


            var emp = await _context.Empresa.Where(e => e.Id == id).FirstAsync();

            if (emp == null)
                return NotFound();
           
            emp.ativo = empresa.ativo;
            var users = await _userManager.Users.ToListAsync();
            
            foreach (var user in users) {
                if (user.EmpresaId == emp.Id)
                {
                    if (emp.ativo == true)
                    {
                        user.ativo = true;
                    }
                    else
                    {
                        user.ativo = false;
                    }
                }
            }

            _context.Update(emp);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Empresas/Create
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empresas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([Bind("Id,Nome,avaliacao")] Empresa empresa)
        {
            empresa.ativo = true;

            if (ModelState.IsValid)
            {
                _context.Add(empresa);
                await _context.SaveChangesAsync();
                
                var empresacriada = await _context.Empresa.Where(e => e.Nome == empresa.Nome).FirstOrDefaultAsync();

                var GestorEmpresaCriada = new Utilizador
                {
                    PrimeiroNome = "Gestor[" + empresa.Nome + "]",
                    UltimoNome = "Gestor",
                    NIF = 000000000,
                    ativo=true,
                    EmpresaId = empresacriada.Id,
                    Empresa = empresacriada,
                    UserName = empresa.Nome + "@gestores.com",
                    Email = empresa.Nome + "@gestores.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                var checkunique = await _userManager.FindByEmailAsync(GestorEmpresaCriada.Email);

                if (checkunique == null) {
                    await _userManager.CreateAsync(GestorEmpresaCriada, "Gestor1.");
                    await _userManager.AddToRoleAsync(GestorEmpresaCriada, Roles.Gestor.ToString());
                }
                return RedirectToAction(nameof(Index));
            }
            return View(empresa);
        }

        // GET: Empresas/Edit/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Empresa == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresa.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }
            return View(empresa);
        }

        // POST: Empresas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,avaliacao")] Empresa empresa)
        {
            if (id != empresa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empresa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpresaExists(empresa.Id))
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
            return View(empresa);
        }

        // GET: Empresas/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Empresa == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        // POST: Empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Empresa == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Empresa'  is null.");
            }
            var empresa = await _context.Empresa.FindAsync(id);

            //Eliminar se nao tiver veiculos
            var veiculos = await _context.veiculos.ToListAsync();

            bool VeiculosBool = true;
                foreach (var veiculo in veiculos)
                {
                if (veiculo.EmpresaId == id)
                    VeiculosBool = false;
                }

            if (empresa != null && VeiculosBool)
            {
                //Apagar o gestor associado com o id da empresa

                //TODO DESASSOCIAR TODOS OS UTILIZADORES COM O ID DA EMPRESA
                await deleteUsersAsync(empresa.Id);
                _context.Empresa.Remove(empresa);
                await _context.SaveChangesAsync();
            }
                //Notificar que nao eliminou
                
            return RedirectToAction(nameof(Index));
        }

        private async Task deleteUsersAsync(int id) {
            var users = await _userManager.Users.ToListAsync();
            foreach (var u in users)
            {
                if (u.EmpresaId == id)
                    await _userManager.DeleteAsync(u);
            }
        }

        private bool EmpresaExists(int id)
        {
          return _context.Empresa.Any(e => e.Id == id);
        }
    }
}
