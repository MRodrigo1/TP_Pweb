using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PWEB_AulasP_2223.Data;
using TP_Pweb.Data;
using TP_Pweb.Models;
using TP_Pweb.ViewModels;

namespace TP_Pweb.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly UserManager<Utilizador> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public UserRolesController(UserManager<Utilizador> userManager,
       RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            /* código a criar */
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            /* código a criar */
            var userRolesViewModel = new List<UserRolesViewModel>();
            foreach (Utilizador user in users)
            {
                var thisViewModel = new UserRolesViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.UserName = user.UserName;
                thisViewModel.Avatar = user.Avatar;
                thisViewModel.PrimeiroNome = user.PrimeiroNome;
                thisViewModel.UltimoNome = user.UltimoNome;
                thisViewModel.Roles = await GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);
            }
            return View(userRolesViewModel);
        }

        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> IndexFuncionariosEmpresa()
        {
            var userAtual = await _userManager.GetUserAsync(User);
            var users = await _userManager.Users.ToListAsync();

            var Funcionarios = new UsersEmpresaViewModel();
            Funcionarios.empresaid = (int)userAtual.EmpresaId;

            var userRolesViewModel = new List<UserRolesViewModel>();
            foreach (Utilizador user in users)
            {
                if (user.EmpresaId == Funcionarios.empresaid)
                {
                    var thisViewModel = new UserRolesViewModel();
                    thisViewModel.UserId = user.Id;
                    thisViewModel.UserName = user.UserName;
                    thisViewModel.Avatar = user.Avatar;
                    thisViewModel.PrimeiroNome = user.PrimeiroNome;
                    thisViewModel.UltimoNome = user.UltimoNome;
                    thisViewModel.Roles = await GetUserRoles(user);
                    userRolesViewModel.Add(thisViewModel);
                }
            }

            Funcionarios.ListaFunc = userRolesViewModel;

            return View(Funcionarios);
        }

        private async Task<List<string>> GetUserRoles(Utilizador user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || _userManager.Users == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(string id, [Bind("Id,PrimeiroNome,UltimoNome,DataNascimento,NIF,PhoneNumber")] Utilizador utilizador)
        {
            var utilizadoreditado = await _context.Users.Where(c => c.Id == id).FirstAsync();
            if (id != utilizador.Id)
            {
                return NotFound();
            }
            ModelState.Remove(nameof(utilizador.Empresa));
            utilizadoreditado.PrimeiroNome = utilizador.PrimeiroNome;
            utilizadoreditado.UltimoNome = utilizador.UltimoNome;
            utilizadoreditado.DataNascimento = utilizador.DataNascimento;
            utilizadoreditado.NIF = utilizador.NIF;
            utilizadoreditado.PhoneNumber = utilizador.PhoneNumber;

            if (ModelState.IsValid)
            {
                _context.Update(utilizadoreditado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(utilizador);
        }

        [Authorize(Roles = "Gestor, Administrador")]
        public async Task<IActionResult> AtivaRegistos(string userId)
        {
            var ManageUserRolesViewModel = new ManageUserRolesViewModel();
            var users = await _userManager.Users.ToListAsync();
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.UtilizadorNome = user.PrimeiroNome + " " + user.UltimoNome;
            ManageUserRolesViewModel.active = user.ativo;
            return View(ManageUserRolesViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Gestor, Administrador")]
        public async Task<IActionResult> AtivaRegistos(ManageUserRolesViewModel model, string userId)
        {
            var user = await _context.Users.Where(c => c.Id == userId).FirstAsync();
            if (userId == null)
                return NotFound();

            user.ativo = model.active;

            _context.Update(user);
            await _context.SaveChangesAsync();

            if(User.IsInRole(Roles.Administrador.ToString()))
                return RedirectToAction("Index");
            else
                return RedirectToAction("IndexFuncionariosEmpresa");
        }


        [Authorize(Roles = "Gestor")]
        public IActionResult addGestor()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> addGestor([Bind("PrimeiroNome,UltimoNome,DataNascimento,NIF,Email")] Utilizador utilizador)
        {
            ModelState.Remove(nameof(utilizador.Empresa));
            if (ModelState.IsValid) {
                var userAtual = await _userManager.GetUserAsync(User);
                var empresaatual = _context.Empresa.Where(c => c.Id == userAtual.EmpresaId).FirstOrDefault();

                var Gestornew = new Utilizador
                {
                    Email = utilizador.Email,
                    UserName = utilizador.Email,
                    PrimeiroNome = utilizador.PrimeiroNome,
                    UltimoNome = utilizador.UltimoNome,
                    DataNascimento = utilizador.DataNascimento,
                    NIF = utilizador.NIF,
                    ativo = true,
                    EmpresaId = userAtual.EmpresaId,
                    Empresa = empresaatual,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                var checkunique = await _userManager.FindByEmailAsync(Gestornew.Email);

                if (checkunique == null)
                {
                    await _userManager.CreateAsync(Gestornew, "Gestor1.");
                    await _userManager.AddToRoleAsync(Gestornew, Roles.Gestor.ToString());
                    await _context.SaveChangesAsync();
                    return RedirectToAction("IndexFuncionariosEmpresa");

                }
                else
                    return View(Gestornew);
            }
            return View();
        }

        [Authorize(Roles = "Gestor")]
        public IActionResult addFuncionario()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> addFuncionario([Bind("PrimeiroNome,UltimoNome,DataNascimento,NIF,Email")] Utilizador utilizador)
        {
            ModelState.Remove(nameof(utilizador.Empresa));
            if (ModelState.IsValid)
            {
                var userAtual = await _userManager.GetUserAsync(User);
                var empresaatual = _context.Empresa.Where(c => c.Id == userAtual.EmpresaId).FirstOrDefault();

                var FuncionarioNew = new Utilizador
                {
                    Email = utilizador.Email,
                    UserName = utilizador.Email,
                    PrimeiroNome = utilizador.PrimeiroNome,
                    UltimoNome = utilizador.UltimoNome,
                    DataNascimento = utilizador.DataNascimento,
                    NIF = utilizador.NIF,
                    ativo = true,
                    EmpresaId = userAtual.EmpresaId,
                    Empresa = empresaatual,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };
                var checkunique = await _userManager.FindByEmailAsync(FuncionarioNew.Email);

                if (checkunique == null)
                {
                    await _userManager.CreateAsync(FuncionarioNew, "Funcionario1.");
                    await _userManager.AddToRoleAsync(FuncionarioNew, Roles.Funcionario.ToString());
                    await _context.SaveChangesAsync();
                    return RedirectToAction("IndexFuncionariosEmpresa");

                }
                else
                    return View(FuncionarioNew);
            }
            return View();
        }
    }
}