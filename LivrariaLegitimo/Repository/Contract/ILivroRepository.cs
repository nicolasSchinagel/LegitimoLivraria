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


    }
}
