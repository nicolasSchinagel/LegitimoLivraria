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

        IEnumerable<Livro> ObterMaisVendidos();

        IEnumerable<Livro> ObterPorLiteratura(string literatura);

        IEnumerable<Livro> ObterPorPesquisa(string nome);

        IEnumerable<Livro> ObterPorHistoricoG(List<string> codBarrasG);

        IEnumerable<Livro> ObterPorHistoricoE(List<string> codBarrasE);

        void AtualizarHistoricoG(Livro livro);
        void AtualizarHistoricoE(Livro livro);

    }
}
