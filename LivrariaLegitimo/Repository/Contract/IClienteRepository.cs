using LivrariaLegitimo.Models;
using X.PagedList;

namespace LivrariaLegitimo.Repository.Contract
{
    public interface IClienteRepository
    {
        Cliente Login(string Email, string Senha);
        Cliente ObterCliente(int Id);

        void AtualizarCliente(Cliente cliente);
    }
}
