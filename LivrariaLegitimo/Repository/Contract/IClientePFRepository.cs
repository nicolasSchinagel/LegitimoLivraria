using LivrariaLegitimo.Models;

namespace LivrariaLegitimo.Repository.Contract
{
    public interface IClientePFRepository
    {
        void Cadastrar(ClientePF clientePF);
        Int64 ObterCPF(int idCli);

    }
}
