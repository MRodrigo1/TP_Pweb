using Microsoft.AspNetCore.Mvc;

namespace TP_Pweb.ViewModels
{
    public class ManageUserRolesViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool Selected { get; set; }
        public bool active { get; set; }
    }
}
