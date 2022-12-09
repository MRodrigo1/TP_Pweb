namespace TP_Pweb.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public ICollection<Veiculo>? veiculos { get; set; }
    }
}
