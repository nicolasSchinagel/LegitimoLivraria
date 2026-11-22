using LivrariaLegitimo.Models;
using X.PagedList;

namespace LivrariaLegitimo.Repository.Contract
{
    public interface IVendaRepository
    {
        public void Cadastrar(Venda venda);
        IEnumerable<ItemVenda> ObterListaPorNumero(int numeroVenad);
    }
}
