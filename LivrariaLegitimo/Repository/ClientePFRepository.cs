using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository.Contract;
using MySql.Data.MySqlClient;
using System.Data;
using X.PagedList;

namespace LivrariaLegitimo.Repository
{
    public class ClientePFRepository : IClientePFRepository
    {
        private readonly string _conexaoMySQL;
        IConfiguration _config;
        public ClientePFRepository(IConfiguration conf)
        {
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
            _config = conf;
        }
        

        public void Cadastrar(ClientePF clientePF)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("call spCadastroClientePF(@Nome, @CPF , @Email, @Telefone , @Senha , @CEP, @NumeroEndereco, @Complemento, @Logradouro, @Bairro, @Cidade, @Estado, @DataNascimento );", conexao);

                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = clientePF.Nome;
                cmd.Parameters.Add("@DataNascimento", MySqlDbType.Date).Value = clientePF.DataNasc.ToString("yyyy/MM/dd");
                cmd.Parameters.Add("@CPF", MySqlDbType.Int64).Value = Convert.ToInt64(clientePF.Cpf.Replace("-", "").Replace(".", ""));
                cmd.Parameters.Add("@Telefone", MySqlDbType.Int64).Value = Convert.ToInt64(clientePF.Telefone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", ""));
                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = clientePF.Email;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = clientePF.Senha;
                cmd.Parameters.Add("@CEP", MySqlDbType.VarChar).Value = clientePF.Cep.Replace("-", "");
                cmd.Parameters.Add("@NumeroEndereco", MySqlDbType.Int32).Value = clientePF.NumeroEndereco;
                cmd.Parameters.Add("@Complemento", MySqlDbType.VarChar).Value = clientePF.ComplementoEndereco;
                cmd.Parameters.Add("@Logradouro", MySqlDbType.VarChar).Value = clientePF.Logradouro;
                cmd.Parameters.Add("@Bairro", MySqlDbType.VarChar).Value = clientePF.Bairro;
                cmd.Parameters.Add("@Cidade", MySqlDbType.VarChar).Value = clientePF.Cidade;
                cmd.Parameters.Add("@Estado", MySqlDbType.VarChar).Value = clientePF.Estado;



                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public Int64 ObterCPF(int idCli)
        {
            Int64 cpf = 0;
            using(var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbClientePF where(idCli = @IdCli);", conexao);
                cmd.Parameters.AddWithValue("@IdCli", idCli);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    cpf = (Int64)(dr["Cpf"]);
                }
                return cpf;
            }
        }
    }
}
