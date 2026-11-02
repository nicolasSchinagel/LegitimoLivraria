using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository.Contract;
using X.PagedList;
namespace LivrariaLegitimo.Repository
{
    public class FuncionarioRepository : IFuncionarioRepository
    {
        public void Atualizar(Funcionario funcionario)
        {
            throw new NotImplementedException();
        }

        public void Cadastrar(Funcionario funcionario)
        {
            throw new NotImplementedException();
        }

        public Funcionario Login(string Email, string Senha)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Funcionario> ObterTodosFuncionarios()
        {
            throw new NotImplementedException();
        }

        public IPagedList<Funcionario> ObterTodosFuncionarios(int? pagina, string pesquisa)
        {
            throw new NotImplementedException();
        }
    }
}
