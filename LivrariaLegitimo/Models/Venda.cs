using System.ComponentModel.DataAnnotations;

namespace LivrariaLegitimo.Models
{
    public class Venda
    {
        [Display(Name = "ID Venda")]
        public int IdCli { get; set; }

        [Display(Name = "Número da Venda")]
        public int NumVenda { get; set; }

        [Display(Name = "Número da Nota Fiscal")]
        public int NotaFiscal { get; set; }

        [Display(Name = "ID Funcionário")]
        public int? IdFuncionario { get; set; }

        [Display(Name = "Data da Venda")]
        public DateTime DataVenda { get; set; }

        [Display(Name = "Forma de Pagamento")]
        public string FormaPagamento { get; set; }

        [Display(Name = "Número do Cartão")]
        public long? NumeroCartao { get; set; }

        [Display(Name = "Nome do Cartão")]
        public string? NomeCartao { get; set; }

        [Display(Name = "Validade")]
        public DateTime? ValidadeCartao { get; set; }

        [Display(Name = "CVV")]
        public int? Cvv { get; set; }

        [Display(Name = "Número de Parcelamentos")]
        public short NumeroParcelamentos { get; set; }

        public List<ItemVenda> Itens { get; set; } = new List<ItemVenda>();
    }
    public class ItemVenda
    {
        [Display(Name = "Número da Venda")]
        public int NumVenda { get; set; }

        [Display(Name = "Valor do Item")]
        public decimal ValorItem { get; set; }

        [Display(Name = "Quantidade do Item")]
        public int QtdItem { get; set; }

        [Display(Name = "Código de Barras")]
        public decimal? CodBarras { get; set; }
    }
}
