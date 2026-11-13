using LivrariaLegitimo.Models;
using X.PagedList;

namespace LivrariaLegitimo.Repository.Contract
{
    public interface ILivroRepository
    {
        public void Cadastrar(Livro livro);
        public void ModificarPreco(int promocao, decimal preco);

        Livro ObterLivro(string CodBarras);

        IEnumerable<Livro> ObterTodosLivros();

        IEnumerable<Livro> ObterMelhoresAvaliados();

        IEnumerable<Livro> ObterMaisVendidos();
        IPagedList<Livro> ObterMaisVendidos(int? pagina, string pesquisa);

        IEnumerable<Livro> ObterPorLiteratura(string literatura);

        IEnumerable<Livro> ObterPorPesquisa(string nome);
        IPagedList<Livro> ObterPorPesquisa(int? pagina, string pesquisa);

        IEnumerable<Livro> ObterPorHistoricoG(List<string> codBarrasG);
        IPagedList<Livro> ObterPorHistoricoG(int? pagina, string pesquisa);

        IEnumerable<Livro> ObterPorHistoricoE(List<string> codBarrasE);
        IPagedList<Livro> ObterPorHistoricoE(int? pagina, string pesquisa);

        void AtualizarHistoricoG(Livro livro);
        void AtualizarHistoricoE(Livro livro);

    }
}
