namespace TP_Pweb.Models
{
  
    public class Estado
    {
        public int Id { get; set; }
        public Boolean Concluido { get; set; }
        public int NrKilometros { get; set; }
        public Boolean danos { get; set; }
        public Boolean danos2 { get; set; }
        public string observacoes { get; set; }
        //Relações
        public string? UtilizadorId { get; set; }
        public Utilizador Funcionario { get; set; }


    }
}
