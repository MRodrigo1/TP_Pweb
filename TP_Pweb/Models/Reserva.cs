namespace TP_Pweb.Models
{
    public class Reserva
    {
        public enum State
        {
            Pendente,
            Decorrer,
            Cancelada
        }
        public int Id { get; set; }
        public State state { get; set; }
        public DateTime DataEntrega { get; set; }
        public DateTime DataRecolha { get; set; }
        public string UtilizadorId { get; set; }
        public Utilizador Utilizador { get; set; }
        public int VeiculoId { get; set; }
        public Veiculo Veiculo { get; set; }
        public ICollection<Estado> estados { get; set; }
    }
}
