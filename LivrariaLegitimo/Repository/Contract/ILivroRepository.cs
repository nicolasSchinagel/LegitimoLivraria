using LivrariaLegitimo.Models;
using X.PagedList;

namespace LivrariaLegitimo.Repository.Contract
{
    public interface ILivroRepository
    {
        public void Cadastrar(Livro livro);
        public void ModificarPreco(int promocao, decimal preco, decimal codBarras);

        public (Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas) ObterLivro(string codBarras);

        IEnumerable<(Livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> ObterTodosLivros(string pesquisa);


        IEnumerable<(Livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> ObterMelhoresAvaliados();

        IEnumerable<(Livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> AleatorizarLivros(string? codBarras = null);

        IEnumerable<(Livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> ObterMaioresOfertas();

        IEnumerable<(Livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> ObterMaisVendidos();

        bool EmAlta(string codBarras);

        public void AdicionarEstoque(List<(decimal codBarras, int quantidade)> lista);

        public (int totalLivros, int romance, int aventura, int fantasia, int ficcaoCientifica, int historia, int biografia, int autoajuda, int terror, int misterio, int filosofia, int humor, int drama, int suspense, int religiao, int literaturaClassica, int literaturaInfantil, int literaturaJuvenil, int contos, int poesia, int mangas, int totalAssociacoesGeneros) ObterQuantidadeGenero();

    }
}
