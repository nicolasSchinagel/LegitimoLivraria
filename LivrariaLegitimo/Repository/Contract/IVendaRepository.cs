using LivrariaLegitimo.Models;
using X.PagedList;

namespace LivrariaLegitimo.Repository.Contract
{
    public interface IVendaRepository
    {
        public void Cadastrar(Venda venda);
        IEnumerable<ItemVenda> ObterListaPorId(int vendaId);
        IPagedList<ItemVenda> ObterListaPorId(int? pagina, string pesquisa);
    }
}
