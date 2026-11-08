using LivrariaLegitimo.Libraries.Sessao;
using LivrariaLegitimo.Models;
using Newtonsoft.Json;
using LivrariaLegitimo.Libraries.Sessao;
using LivrariaLegitimo.Models;
namespace LivrariaLegitimo.Libraries.Login
{
    public class LoginFuncionario
    {
        private string Key = "Login.Funcionario";
        private Sessao.Sessao _sessao;
        public LoginFuncionario(Sessao.Sessao sessao)
        {
            _sessao = sessao;
        }

        public void Login(Funcionario funcionario)
        {
            //serializar
            string clienteJSONString = JsonConvert.SerializeObject(funcionario);

            _sessao.Cadastrar(Key, clienteJSONString);
        }

        public Funcionario GetCliente()
        {
            //deserializar
            if (_sessao.Existe(Key))
            {
                string funcionarioJSONString = _sessao.Consultar(Key);
                return JsonConvert.DeserializeObject<Funcionario>(funcionarioJSONString);
            }
            else
            {
                return null;
            }
        }
        public void Logout()
        {
            _sessao.RemoverTodos();
        }
    }
}
