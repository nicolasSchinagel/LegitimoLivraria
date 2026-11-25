using LivrariaLegitimo.Models;
namespace LivrariaLegitimo.ViewModels
{
    public class CompraViewModel
    {
        public List<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas, int quantidadeCarrinho)> Carrinho { get; set; }
        public Venda Compra { get; set; }
        public Cliente ObterCliente { get; set; }
        public IEnumerable<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> TodosLivros { get; set; }
    }
}
