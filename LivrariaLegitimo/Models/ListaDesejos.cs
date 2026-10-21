using System.ComponentModel.DataAnnotations;

namespace LivrariaLegitimo.Models
{
    public class ListaDesejos
    {
        public List<LivroDesejos> Livros { get; set; }
    }
    public class LivroDesejos
    {
        [Display(Name = "Código de Barras")]
        public string CodigoBarras { get; set; }
    }
}
