namespace TP_Pweb.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public Boolean Concluida { get; set; }

        //Relacoes
        //public int EstadoEntregaId { get; set; }
        //public Estado EstadoEntrega { get; set; }
        ////public int EstadoRecolhaId { get; set; }
        ////public Estado EstadoRecolha { get; set; }
        public string UtilizadorId { get; set; }
        public Utilizador Utilizador { get; set; }

        public string VeiculoId { get; set; }
        public Veiculo Veiculo { get; set; }
    }
}
