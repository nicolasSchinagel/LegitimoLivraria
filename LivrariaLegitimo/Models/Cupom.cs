using System.ComponentModel.DataAnnotations;

namespace LivrariaLegitimo.Models
{
    public class Cupom
    {
        [Display(Name = "Código do Cupom")]
        public string CodigoCupom { get; set; }

        [Display(Name = "Prazo de Validade")]
        public DateTime Prazo { get; set; }

        [Display(Name = "Promoção do cupom")]
        [Range(0, 90)]
        public string Promocao { get; set; }
    }
}
