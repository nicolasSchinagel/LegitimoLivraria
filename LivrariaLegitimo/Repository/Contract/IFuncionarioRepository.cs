using LivrariaLegitimo.Models;
using LivrariaLegitimo.ViewModels;
using X.PagedList;

namespace LivrariaLegitimo.Repository.Contract
{
    public interface IFuncionarioRepository
    {
        Funcionario Login(string Email, string Senha);

        void Cadastrar(Funcionario funcionario);
        void Atualizar(FuncionarioEditarViewModel funcionario);

        Funcionario ObterFuncionario(int idFuncionario);
        IEnumerable<Funcionario> ObterTodosFuncionarios();
    }
}
