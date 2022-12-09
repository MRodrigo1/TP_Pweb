using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TP_Pweb.Models;

namespace TP_Pweb.Data
{
    public class ApplicationDbContext : IdentityDbContext<Utilizador>
    {
        public DbSet<Veiculo> veiculos { get; set; }
        public DbSet<Categoria> categorias { get; set; }
        public DbSet<Reserva> reservas { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}