using LivrariaLegitimo.Models;

namespace LivrariaLegitimo.Repository.Contract
{
    public interface IClientePJRepository
    {
        void Cadastrar(ClientePJ clientePJ);

        int ObterCNPJ(int Idcli);
    }
}
