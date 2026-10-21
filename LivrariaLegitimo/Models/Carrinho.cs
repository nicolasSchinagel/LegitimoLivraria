using System.ComponentModel.DataAnnotations;

namespace LivrariaLegitimo.Models
{
    public class Carrinho
    {
        public List<LivroCarrinho> LivroCarrinho { get; set; } = new();
    }
    public class LivroCarrinho
    {
        [Display(Name = "Código de barras")]
        public int CodigoBarras { get; set; }

        [Display(Name = "Quantidade Carrinho")]
        public int QtdCarrinho { get; set; }
    }
}
