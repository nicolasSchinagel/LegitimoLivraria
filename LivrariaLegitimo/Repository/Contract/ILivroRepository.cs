using LivrariaLegitimo.Models;
using X.PagedList;

namespace LivrariaLegitimo.Repository.Contract
{
    public interface ILivroRepository
    {
        public void Cadastrar(Livro livro);
        public void ModificarPreco(int promocao, decimal preco);

        (Livro, string caminhoImagem, string caminhoEbook) ObterLivro(string CodBarras);

        IEnumerable<(Livro, string arquivoImagem, string arquivoEbook)> ObterTodosLivros();

        IEnumerable<Livro> ObterMelhoresAvaliados();

        IEnumerable<(Livro, string arquivoImagem, string arquivoEbook)> AleatorizarLivros();

        IEnumerable<(Livro, string arquivoImagem, string arquivoEbook)> ObterMaioresOfertas();

        IEnumerable<(Livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> ObterMaisVendidos();

        IEnumerable<(Livro, string arquivoImagem, string arquivoEbook)> ObterPorLiteratura(string literatura);
        IEnumerable<(Livro, string arquivoImagem, string arquivoEbook)> ObterPorCategoria(string categoria); 


        IEnumerable<Livro> ObterPorPesquisa(string nome); // ordena baseado em nomes, editoras e autores aproximados

        IEnumerable<(Livro, string arquivoImagem, string arquivoEbook)> ObterPorHistoricoG(List<string> codBarrasG);

        IEnumerable<(Livro, string arquivoImagem, string arquivoEbook)> ObterPorHistoricoE(List<string> codBarrasE);

        IEnumerable<Livro> FiltroComplexo();

        void AtualizarHistoricoG(Livro livro);
        void AtualizarHistoricoE(Livro livro);

    }
}
