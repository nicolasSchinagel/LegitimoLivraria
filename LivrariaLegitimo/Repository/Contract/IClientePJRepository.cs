using LivrariaLegitimo.Models;

namespace LivrariaLegitimo.Repository.Contract
{
    public interface IClientePJRepository
    {
        void Cadastrar(ClientePJ clientePJ);

        Int64 ObterCNPJ(int idCli);
    }
}
