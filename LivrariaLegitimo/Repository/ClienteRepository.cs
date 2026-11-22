using LivrariaLegitimo.Libraries.Login;
using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository.Contract;
using MySql.Data.MySqlClient;
using System.Data;
using X.PagedList;

namespace LivrariaLegitimo.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly string _conexaoMySQL;
        IConfiguration _config;
        private LoginFuncionario _loginFuncionario;
        private LoginCliente _loginCliente;
        public ClienteRepository(IConfiguration conf, LoginCliente loginCliente, LoginFuncionario loginFuncionario)
        {
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
            _config = conf;
            _loginCliente = loginCliente;
            _loginFuncionario = loginFuncionario;
        }

        public void AtualizarCliente(Cliente cliente)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("call spAtualizarCliente(@IdCli, @Senha, @NumTel, @Email, @CEP, @NumeroEndereco, @Complemento, @Logradouro, @Bairro, @Cidade, @Estado);", conexao);
                cmd.Parameters.AddWithValue("@IdCli", cliente.IdCli);
                cmd.Parameters.AddWithValue("@Senha", cliente.Senha);
                cmd.Parameters.AddWithValue("@NumTel", Convert.ToInt64(cliente.Telefone));
                cmd.Parameters.AddWithValue("@Email", cliente.Email);
                cmd.Parameters.AddWithValue("@CEP", cliente.Cep.Replace("-", ""));
                cmd.Parameters.AddWithValue("@NumeroEndereco", cliente.NumeroEndereco);
                cmd.Parameters.AddWithValue("@Complemento", cliente.ComplementoEndereco);
                cmd.Parameters.AddWithValue("@Logradouro", cliente.Logradouro);
                cmd.Parameters.AddWithValue("@Bairro", cliente.Bairro);
                cmd.Parameters.AddWithValue("@Cidade", cliente.Cidade);
                cmd.Parameters.AddWithValue("@Estado", cliente.Estado);
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }


        public Cliente Login(string Email, string Senha)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from ViewCliente where Email = @Email and Senha = @Senha", conexao);

                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = Email;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = Senha;

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Cliente cliente = new ClientePF();
                dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    cliente.IdCli = Convert.ToInt32(dr["IdCli"]);
                    cliente.Nome = Convert.ToString(dr["Nome"]);
                    cliente.Email = Convert.ToString(dr["Email"]);
                    cliente.Senha = Convert.ToString(dr["Senha"]);
                    cliente.Telefone = Convert.ToString(dr["NumTel"]);
                    cliente.Cep = Convert.ToString(dr["Cep"]);
                    cliente.Logradouro = Convert.ToString(dr["Logradouro"]);
                    cliente.Bairro = Convert.ToString(dr["Bairro"]);
                    cliente.Cidade = Convert.ToString(dr["Cidade"]);
                    cliente.Estado = Convert.ToString(dr["Estado"]);
                    cliente.NumeroEndereco = Convert.ToInt32(dr["NumeroEndereco"]);
                    cliente.ComplementoEndereco = Convert.ToString(dr["ComplementoEndereco"]);
                }
                return cliente;
            }
        }

        public Cliente ObterCliente(int id)
        {
            using(var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from ViewCliente where @IdCli = idCli", conexao);
                cmd.Parameters.AddWithValue("@IdCli", id);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Cliente cliente = new Cliente();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while(dr.Read()){
                    cliente.IdCli = (Int32)(dr["idCli"]);
                    cliente.Nome = (string)(dr["Nome"]);
                    cliente.Senha = (string)(dr["Senha"]);
                    cliente.Telefone = (string)(dr["NumTel"]);
                    cliente.Email = (string)(dr["Email"]);
                    cliente.Cep = (string)(dr["Cep"]);
                    cliente.NumeroEndereco = (Int32)(dr["NumeroEndereco"]);
                    cliente.ComplementoEndereco = (string)(dr["ComplementoEndereco"]);
                    cliente.Logradouro = (string)(dr["Logradouro"]);
                    cliente.Bairro = (string)(dr["Bairro"]);
                    cliente.Cidade = (string)(dr["Cidade"]);
                    cliente.Estado = (string)(dr["Estado"]);
                }
                return cliente;
            }
        }

    }
}
