using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository.Contract;
using MySql.Data.MySqlClient;
using X.PagedList;

namespace LivrariaLegitimo.Repository
{
    public class ClientePFRepository : IClienteRepository
    {
        private readonly string _conexaoMySQL;
        IConfiguration _config;
        public ClientePFRepository(IConfiguration conf)
        {
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
            _config = conf;
        }
        public void Atualizar(Cliente cliente)
        {
            throw new NotImplementedException();
        }

        public void Cadastrar(Cliente cliente)
        {
            throw new NotImplementedException();
        }

        public Cliente Login(string Email, string Senha)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbClientePFG where Email = @Email and Senha = @Senha", conexao);

                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = Email;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = Senha;

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                ClientePF clientePF = new ClientePF();
                dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    clientePF.IdCli = Convert.ToInt32(dr["Id"]);
                    clientePF.Nome = Convert.ToString(dr["Nome"]);
                    clientePF.DataNasc = DateOnly.Parse(dr["Nascimento"].ToString());
                    clientePF.Cep = Convert.ToInt32(dr["Cep"]);
                    clientePF.NumeroEndereco = Convert.ToInt32(dr["NumeroEndereco"]);
                    clientePF.Cpf = Convert.ToInt32(dr["CPF"]);
                    clientePF.Telefone = Convert.ToInt32(dr["Telefone"]);
                    clientePF.ComplementoEndereco = Convert.ToString(dr["ComplementoEndereco"]);
                    clientePF.Logradouro = Convert.ToString(dr["Logradouro"]);
                    clientePF.Bairro = Convert.ToString(dr["Bairro"]);
                    clientePF.Cidade = Convert.ToString(dr["Cidade"]);
                    clientePF.Estado = Convert.ToString(dr["Estado"]);
                    clientePF.Email = Convert.ToString(dr["Email"]);
                    clientePF.Senha = Convert.ToString(dr["Senha"]);
                }
                return clientePF;
            }
        }

        public Cliente ObterCliente(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Cliente> ObterTodosClientes()
        {
            throw new NotImplementedException();
        }

        public IPagedList<Cliente> ObterTodosClientes(int? pagina, string pesquisa)
        {
            throw new NotImplementedException();
        }
    }
}
