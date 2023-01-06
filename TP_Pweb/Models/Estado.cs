namespace TP_Pweb.Models
{
  
    public class Estado
    {

        public enum State
        {
            Entrega,
            Recolha,
        }
        public int Id { get; set; }
        public State state { get; set; }
        public int NrKilometros { get; set; }
        public Boolean danos { get; set; }
        public string observacoes { get; set; }
        //Relações
        public string? FuncionarioId { get; set; }
        public Utilizador Funcionario { get; set; }
        public int? ReservaId { get; set; }
        public Reserva Reserva { get; set; }


    }
}
