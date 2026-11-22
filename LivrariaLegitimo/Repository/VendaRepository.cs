using LivrariaLegitimo.Libraries.Login;
using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository.Contract;
namespace LivrariaLegitimo.Repository
{
    public class VendaRepository : IVendaRepository
    {
        private readonly string _conexaoMySQL;
        IConfiguration _config;
        private readonly LoginCliente _loginCliente;
        private readonly LoginFuncionario _loginFuncionario;
        public VendaRepository(LoginCliente loginCliente, LoginFuncionario loginFuncionario, IConfiguration conf)
        {
            _loginCliente = loginCliente;
            _loginFuncionario = loginFuncionario;
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
            _config = conf;
        }

        public void Cadastrar(Venda venda)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ItemVenda> ObterListaPorNumero(int numeroVenda)
        {
            throw new NotImplementedException();
        }
    }
}
