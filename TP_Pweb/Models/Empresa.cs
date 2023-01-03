using System.ComponentModel.DataAnnotations;

namespace TP_Pweb.Models
{
    public class Empresa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int avaliacao { get; set; }
        public Boolean ativo { get; set; }
        public ICollection<Veiculo>? Veiculos { get; set; }
        public ICollection<Utilizador>? Funcionarios { get; set; }

    }
}
