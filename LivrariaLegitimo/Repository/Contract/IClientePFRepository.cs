using LivrariaLegitimo.Models;

namespace LivrariaLegitimo.Repository.Contract
{
    public interface IClientePFRepository
    {
        void Cadastrar(ClientePF clientePF);
        void Atualizar(ClientePF clientePF);
    }
}
