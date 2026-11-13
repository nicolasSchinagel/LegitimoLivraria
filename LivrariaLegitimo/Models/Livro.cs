using System.ComponentModel.DataAnnotations;

namespace LivrariaLegitimo.Models
{
    public class Livro: IValidatableObject
    {
        [Display(Name = "Código de Barras")]
        [Required(ErrorMessage = "Código de barras é obrigatório")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "O Código de Barras deve conter exatamente 13 números")]
        public decimal CodigoBarras { get; set; }

        [Display(Name = "Nome do Livro")]
        [Required(ErrorMessage = "O Nome é obrigatório")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "O nome deve ter entre 1 e 100 caracteres.")]
        public string Nome { get; set; }

        [Display(Name = "Preço")]
        [Required(ErrorMessage = "O Preço é obrigatório")]
        [StringLength(7, MinimumLength = 1, ErrorMessage = "O Preço deve ter pelo menos um número, ou até 6 digitos")]
        [RegularExpression(@"^(?!0+$).+", ErrorMessage = "O Preço precisa ser maior que 0")]
        public string Preco { get; set; }

        [Display(Name = "Autor")]
        [Required(ErrorMessage = "O Autor é obrigatório")]
        public string Autor { get; set; }

        [Display(Name = "Editora")]
        [Required(ErrorMessage = "A Editora é obrigatória")]
        public string Editora { get; set; }

        [Display(Name = "Promoção")]
        [Range(0, 89, ErrorMessage = "A promoção deve estar entre 0 e 89.")]
        public int Promocao { get; set; }

        [Display(Name = "Gêneros")]
        [Required(ErrorMessage = "Selecione pelo menos um gênero")]
        [MinLength(1, ErrorMessage = "Selecione pelo menos um gênero")]
        public List<string> Generos { get; set; } = new();

        [Display(Name = "Idade Recomendada")]
        [Required(ErrorMessage = "A Idade Recomendada é obrigatória")]
        [RegularExpression(@"^\+(3|5|10|14|18)$", ErrorMessage = "Selecione uma idade recomendada válida.")]
        public string IdadeRecomendada { get; set; }

        [Display(Name = "Sinopse")]
        [Required(ErrorMessage = "A Sinopse é obrigatória")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "A sinopse deve ter entre 1 e 500 caracteres.")]
        public string Sinopse { get; set; }

        [Display(Name = "Data de publicação")]
        [Required(ErrorMessage = "A Data de Publicação é obrigatória")]
        public DateOnly DataPubli { get; set; }

        [Display(Name = "Tipo de Capa")]
        [Required(ErrorMessage = "O Tipo de Livro é obrigatório")]
        [RegularExpression(@"^(capa dura|capa mole|e-book)$", ErrorMessage = "Selecione um tipo de livro válido.")]
        public string TipoCapa { get; set; }

        [Display(Name = "Idioma")]
        [Required(ErrorMessage = "O Idioma é obrigatório")]
        [RegularExpression(@"^(português|inglês|espanhol)$", ErrorMessage = "Selecione um idioma válido.")]
        public string Idioma { get; set; }

        [Display(Name = "Tamanho do Livro")]
        [Required(ErrorMessage = "O Tamanho do Livro é obrigatório")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "O tamanho deve ter entre 1 e 100 caracteres.")]
        public string Tamanho { get; set; }

        [Display(Name = "Capacidade do Estoque")]
        [Required(ErrorMessage = "A Capacidade do Estoque é obrigatória")]
        [Range(10, 100, ErrorMessage = "A capacidade deve estar entre 10 e 100.")]
        public int Capacidade { get; set; }

        [Display(Name = "Quantidade")]
        [Range(0, int.MaxValue, ErrorMessage = "A quantidade deve ser um número positivo.")]
        public int Quantidade { get; set; }

        [Display(Name = "ISBN")]
        [Required(ErrorMessage = "O ISBN é obrigatório")]
        [RegularExpression(@"^\d{3}-\d{2}-\d{4}-\d{3}-\d{1}$", ErrorMessage = "O ISBN deve conter 13 números")]
        public string ISBN { get; set; }

        [Display(Name = "Literatura")]
        [Required(ErrorMessage = "A Literatura é obrigatória")]
        [RegularExpression(@"^(internacional|nacional)$", ErrorMessage = "Selecione uma literatura válida.")]
        public string Literatura { get; set; }

        [Display(Name = "Imagem da Capa")]
        [Required(ErrorMessage = "A Imagem da Capa é obrigatória")]
        [RegularExpression(@"^.+\.(jpg|jpeg|png|webp)$", ErrorMessage = "A imagem deve estar no formato JPG, JPEG, PNG ou WebP.")]
        public IFormFile Imagem { get; set; }

        [Display(Name = "Arquivo do Ebook")]
        [RegularExpression(@"^.+\.pdf$", ErrorMessage = "O arquivo deve estar no formato PDF.")]
        public IFormFile? ArquivoEbook { get; set; }

        [Display(Name = "Desconto do Livro")]
        public int PromocaoPorcentagem { get; set; }






        // Teste para obrigar arquivos Ebook quando tipo for Ebook
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (TipoCapa == "e-book" && ArquivoEbook == null)
            {
                yield return new ValidationResult(
                    "O arquivo do Ebook é obrigatório para livros digitais.",
                    new[] { nameof(ArquivoEbook) }
                );
            }
        }
    }



}