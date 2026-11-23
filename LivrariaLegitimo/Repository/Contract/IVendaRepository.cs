using LivrariaLegitimo.Models;
using X.PagedList;

namespace LivrariaLegitimo.Repository.Contract
{
    public interface IVendaRepository
    {
        public void Cadastrar(Venda venda);
        public (string tipo, bool existencia) ExisteVenda(int idCli, decimal codBarras);
        IEnumerable<ItemVenda> ObterListaPorNumero(int numeroVenad);
    }
}
