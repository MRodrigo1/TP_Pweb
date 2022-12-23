using System.ComponentModel.DataAnnotations;
using TP_Pweb.Models;

namespace TP_Pweb.ViewModels
{
    public class CreateReservaViewModel
    {
        
        public string UtilizadorId { get; set; }
        public string VeiculoId { get; set; }
        public DateTime DataInicialPesquisa { get; set; }
        public DateTime DataFinalPesquisa { get; set; }
    }
}
