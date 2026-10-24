using LivrariaLegitimo.Models;
using X.PagedList;

namespace LivrariaLegitimo.Repository.Contract
{
    public interface IListaDesejosRepository
    {
        public void Adicionar(ListaDesejos listadesejos);
        public void Remover(int id);
        IEnumerable<ListaDesejos> ObterTodasListas();
        IPagedList<ListaDesejos> ObterTodasListas(int? pagina, string pesquisa);
    }
}
