using LivrariaLegitimo.Models;

namespace LivrariaLegitimo.Repository.Contract
{
    public interface IClientePJRepository
    {
        void Cadastrar(ClientePJ clientePJ);
        void Atualizar(ClientePJ clientePJ);
    }
}
