using LivrariaLegitimo.Models;
namespace LivrariaLegitimo.ViewModels
{
    public class CompraViewModel
    {
        public List<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas, int quantidadeCarrinho)> Carrinho { get; set; }
        public (string CEP, string logradouro, string bairro, string cidade, string estado, int numeroEndereco, string complementoEndereco) EditarEndereco { get; set; }
        public Venda Compra { get; set; }
    }
}
