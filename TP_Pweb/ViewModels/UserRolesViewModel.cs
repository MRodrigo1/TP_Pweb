using Microsoft.AspNetCore.Mvc;

namespace TP_Pweb.ViewModels
{
    public class UserRolesViewModel
    {
        public byte[]? Avatar { get; set; }
        public string UserId { get; set; }
        public string PrimeiroNome { get; set; }
        public string UltimoNome { get; set; }
        public string UserName { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
