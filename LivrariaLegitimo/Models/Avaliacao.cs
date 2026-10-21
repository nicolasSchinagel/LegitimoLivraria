using System.ComponentModel.DataAnnotations;

namespace LivrariaLegitimo.Models
{
    public class Avaliacao
    {
        [Display(Name = "ID Cliente")]
        public int IdCliente { get; set; }

        [Display(Name = "Codigo de Barras")]
        public int CodigoBarras { get; set; }

        [Display(Name = "Título da Avaliação")]
        [Required]
        public string TituloAvaliacao { get; set; }

        [Display(Name = "Comentário")]
        [Required]
        public string Comentario { get; set; }

        [Display(Name = "Nota do livro")]
        [Required]
        public int NotaLivro { get; set; }



    }
}
