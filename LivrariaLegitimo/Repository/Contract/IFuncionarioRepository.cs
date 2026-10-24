using LivrariaLegitimo.Models;
using X.PagedList;

namespace LivrariaLegitimo.Repository.Contract
{
    public interface IFuncionarioRepository
    {
        Funcionario Login(string Email, string Senha);

        void Cadastrar(Funcionario funcionario);
        void Atualizar(Funcionario funcionario);
        IEnumerable<Funcionario> ObterTodosFuncionarios();
        IPagedList<Funcionario> ObterTodosFuncionarios(int? pagina, string pesquisa);
    }
}
