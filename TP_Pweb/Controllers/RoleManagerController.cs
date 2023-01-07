using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TP_Pweb.Controllers
{
    public class RoleManagerController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleManagerController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index()
        {
            return View(await _roleManager.Roles.ToListAsync());
        }
    }
}
