using System.ComponentModel.DataAnnotations;
using TP_Pweb.Models;

namespace TP_Pweb.ViewModels
{
    public class PesquisaVeiculoViewModel
    {
        public List<Veiculo> ListaDeVeiculos {get; set;}
        public int NumResultados {get; set;}

        //[Display(Name = "PESQUISA DE CURSOS ...", Prompt = "introduza o texto a pesquisar")]
        public string LocalizacaoPesquisa { get; set; }

        public string CategoriaPesquisa { get; set; }

        public DateTime DataInicialPesquisa { get; set; }

        public DateTime DataFinalPesquisa { get; set; }
    }
    }
