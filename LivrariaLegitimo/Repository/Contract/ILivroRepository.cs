using LivrariaLegitimo.Models;
using X.PagedList;

namespace LivrariaLegitimo.Repository.Contract
{
    public interface ILivroRepository
    {
        public void Cadastrar(Livro livro);
        public void ModificarPreco(int promocao, decimal preco);

        public (Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas) ObterLivro(string codBarras);

        IEnumerable<(Livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> ObterTodosLivros(string pesquisa);


        IEnumerable<(Livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> ObterMelhoresAvaliados();

        IEnumerable<(Livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> AleatorizarLivros();

        IEnumerable<(Livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> ObterMaioresOfertas();

        IEnumerable<(Livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> ObterMaisVendidos();

        bool EmAlta(string codBarras);

    }
}
