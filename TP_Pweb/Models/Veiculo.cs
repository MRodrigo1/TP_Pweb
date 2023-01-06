namespace TP_Pweb.Models
{
    public class Veiculo
    {
        public int Id { get; set; }
        public byte[]? FotoDisplay { get; set; }
        public string Modelo { get; set; }
        public string Localizacao { get; set; }
        public int CustoDia { get; set; }
        public Boolean ativo { get; set; }
        public int nrKm { get; set; }
        public int? EmpresaId { get; set; }
        public Empresa empresa { get; set; }
        public int? CategoriaId { get; set; }
        public Categoria categoria { get; set; }
        public ICollection<Reserva>? reservas { get; set; }
    }
}
