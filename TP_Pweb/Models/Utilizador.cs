using Microsoft.AspNetCore.Identity;
namespace TP_Pweb.Models
{
    public class Utilizador : IdentityUser
    {
        public byte[]? Avatar { get; set; }
        public string PrimeiroNome { get; set; }
        public string UltimoNome { get; set; }
        public DateTime DataNascimento { get; set; }
        public int NIF { get; set; }
        public Boolean ativo { get; set; } = true;
        public int? EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
        public ICollection<Reserva>? reservas { get; set; }

    }
}
