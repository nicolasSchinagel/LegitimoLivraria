using System.ComponentModel.DataAnnotations;

namespace LivrariaLegitimo.Models
{
    // continuar daqui
    public class Carrinho
    {
        public List<LivroCarrinho> LivroCarrinho { get; set; } = new();
    }
    public class LivroCarrinho: Livro
    {
        [Display(Name = "Quantidade Carrinho")]
        public int QtdCarrinho { get; set; }
    }
}
