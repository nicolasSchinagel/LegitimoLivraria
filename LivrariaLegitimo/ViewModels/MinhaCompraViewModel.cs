namespace LivrariaLegitimo.ViewModels
{
    public class MinhaCompraViewModel
    {
        public int IdCli { get; set; }

        public string Nome { get; set; }

        public string Autor { get; set; }

        public string Editora { get; set; }

        public decimal ISBN { get; set; }

        public string ImagemPri { get; set; }

        public int NotaFiscal { get; set; }

        public decimal PrecoPago { get; set; }

        public DateTime DataCompra { get; set; }

        public int Quantidade { get; set; }
    }
}
