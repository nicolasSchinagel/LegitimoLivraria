using System.ComponentModel.DataAnnotations;

namespace LivrariaLegitimo.Models
{
    public class Avaliacao
    {
       

        [Display(Name = "Código de Barras")]
        [Required]
        public decimal CodigoBarras { get; set; }

        [Display(Name = "Título da Avaliação")]
        [Required(ErrorMessage = "Informe o título da avaliação.")]
        [StringLength(50, ErrorMessage = "O título deve ter no máximo 50 caracteres.")]
        public string TituloAvaliacao { get; set; }

        [Display(Name = "Comentário")]
        [Required(ErrorMessage = "Informe seu comentário.")]
        [StringLength(200, ErrorMessage = "O comentário deve ter no máximo 200 caracteres.")]
        public string Comentario { get; set; }

        [Display(Name = "Nota do livro")]
        [Required]
        public int NotaLivro { get; set; }

        [Display(Name = "Data da avaliação")]
        [Required]
        public DateOnly DataAvaliacao { get; set; }

    }
}
