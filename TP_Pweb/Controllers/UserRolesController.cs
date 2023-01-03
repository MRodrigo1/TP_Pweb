using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        private async Task<List<string>> GetUserRoles(Utilizador user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        public async Task<IActionResult> Details(string userId)
        {
            var ManageUserRolesViewModel = new ManageUserRolesViewModel();
            var users = await _userManager.Users.ToListAsync();
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.UtilizadorNome = user.PrimeiroNome + " " + user.UltimoNome;
            ManageUserRolesViewModel.active = user.ativo;
            return View(ManageUserRolesViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Details(ManageUserRolesViewModel model,string userId)
        {
            
            var user = await _context.Users.Where(c => c.Id == userId).FirstAsync();
            if (userId == null)
                return NotFound();

            user.ativo = model.active;

            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
