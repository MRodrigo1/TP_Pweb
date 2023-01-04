using Microsoft.AspNetCore.Mvc;
using TP_Pweb.Models;

namespace TP_Pweb.ViewModels
{
    public class UsersEmpresaViewModel 
    {
        public List<UserRolesViewModel> ListaFunc { get; set; }

        public int empresaid { get; set; }
    }
}
