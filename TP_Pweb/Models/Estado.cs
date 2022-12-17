namespace TP_Pweb.Models
{
  
    public class Estado
    {
        public int Id { get; set; }
        public int NrKilometros { get; set; }
        public Boolean danos { get; set; }
        public string observacoes { get; set; }
        public DateTime dataReserva { get; set; }
        //Relações
        public int UtilizadorId { get; set; }
        public Utilizador Funcionario { get; set; }


    }
}
