using LivrariaLegitimo.Models;
using X.PagedList;

namespace LivrariaLegitimo.Repository.Contract
{
    public interface ILivroRepository
    {
        public void Cadastrar(Livro livro);
        public void ModificarPreco(int promocao, decimal preco);
        IEnumerable<Livro> ObterTodosLivros();
        IPagedList<Livro> ObterTodosLivros(int? pagina, string pesquisa);

        IEnumerable<Livro> ObterMelhoresAvaliados();
        IPagedList<Livro> ObterMelhoresAvaliados(int? pagina, string pesquisa);

        IEnumerable<Livro> ObterMaisVendidos();
        IPagedList<Livro> ObterMaisVendidos(int? pagina, string pesquisa);

        IEnumerable<Livro> ObterPorLiteratura(string literatura);
        IPagedList<Livro> ObterPorLiteratura(int? pagina, string pesquisa);

        IEnumerable<Livro> ObterPorPesquisa(string nome);
        IPagedList<Livro> ObterPorPesquisa(int? pagina, string pesquisa);

        IEnumerable<Livro> ObterPorHistoricoG(List<string> codBarrasG);
        IPagedList<Livro> ObterPorHistoricoG(int? pagina, string pesquisa);

        IEnumerable<Livro> ObterPorHistoricoE(List<string> codBarrasE);
        IPagedList<Livro> ObterPorHistoricoE(int? pagina, string pesquisa);

        public void AtualizarHistoricoG(Livro livro);
        public void AtualizarHistoricoE(Livro livro);
    }
}
