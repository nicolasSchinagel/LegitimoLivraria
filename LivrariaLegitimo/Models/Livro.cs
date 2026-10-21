using System.ComponentModel.DataAnnotations;

namespace LivrariaLegitimo.Models
{
    public class Livro
    {
        [Display(Name = "Codigo de Barras")]
        public int CodigoBarras { get; set; }

        [Display(Name = "Nome do Livro")]
        public string Nome { get; set; }

        [Display(Name = "Preço")]
        public decimal Preco { get; set; }

        [Display(Name = "Autor")]
        public string Autor { get; set; }

        [Display(Name = "Editora")]
        public string Editora { get; set; }

        [Display(Name = "Promoção")]
        public int Promocao { get; set; }

        [Display(Name = "Genero")]
        // Isso vai ter que ser uma lista, mas vou pensar como fazer esta parte
        public List<string> Generos { get; set; } = new();

        [Display(Name = "Idade Recomendada")]
        public string IdadeRecomendada { get; set; }

        [Display(Name = "Sinopse")]
        public string Sinopse { get; set; }

        [Display(Name = "Data de publicação")]
        public DateOnly DataPubli { get; set; }

        [Display(Name = "Tipo de Capa")]
        public string TipoCapa { get; set; }

        [Display(Name = "Idioma")]
        public string Idioma { get; set; }

        [Display(Name = "Tamanho do Livro")]
        public decimal Tamanho{ get; set; }

        [Display(Name = "Capacidade do Estoque")]
        public int Capacidade { get; set; }

        [Display(Name = "Quantidade")]
        public int Quantidade { get; set; }

        [Display(Name = "ISBN")]
        public int ISBN { get; set; }

        [Display(Name = "Literatura")]
        public string Literatura { get; set; }

        [Display(Name = "Imagem da Capa")]
        public string Imagem { get; set; }

        [Display(Name = "Arquivo do Ebook")]
        public string? ArquivoEbook { get; set; }
    }
}
