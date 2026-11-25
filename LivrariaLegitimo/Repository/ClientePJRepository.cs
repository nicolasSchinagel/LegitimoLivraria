using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository.Contract;
using MySql.Data.MySqlClient;
using System.Data;
using X.PagedList;

namespace LivrariaLegitimo.Repository
{

    public class ClientePJRepository : IClientePJRepository
    {
        private readonly string _conexaoMySQL;
        IConfiguration _config;
        public ClientePJRepository(IConfiguration conf)
        {
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
            _config = conf;
        }

        public void Cadastrar(ClientePJ clientePJ)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("call spCadastroClientePJ(@Nome, @CNPJ , @Email, @Telefone , @Senha , @CEP, @NumeroEndereco, @Complemento, @Logradouro, @Bairro, @Cidade, @Estado );", conexao);

                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = clientePJ.Nome;
                cmd.Parameters.Add("@CNPJ", MySqlDbType.Int64).Value = Convert.ToInt64(clientePJ.Cnpj.Replace("-", "").Replace(".", "").Replace("/", ""));
                cmd.Parameters.Add("@Telefone", MySqlDbType.Int64).Value = Convert.ToInt64(clientePJ.Telefone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", ""));
                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = clientePJ.Email;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = clientePJ.Senha;
                cmd.Parameters.Add("@CEP", MySqlDbType.VarChar).Value = clientePJ.Cep.Replace("-", "");
                cmd.Parameters.Add("@NumeroEndereco", MySqlDbType.Int32).Value = clientePJ.NumeroEndereco;
                cmd.Parameters.Add("@Complemento", MySqlDbType.VarChar).Value = clientePJ.ComplementoEndereco;
                cmd.Parameters.Add("@Logradouro", MySqlDbType.VarChar).Value = clientePJ.Logradouro;
                cmd.Parameters.Add("@Bairro", MySqlDbType.VarChar).Value = clientePJ.Bairro;
                cmd.Parameters.Add("@Cidade", MySqlDbType.VarChar).Value = clientePJ.Cidade;
                cmd.Parameters.Add("@Estado", MySqlDbType.VarChar).Value = clientePJ.Estado;
                



                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public Int64 ObterCNPJ(int idCli)
        {
            Int64 cnpj = 0;
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbClientePJ where(idCli = @IdCli);", conexao);
                cmd.Parameters.AddWithValue("@IdCli", idCli);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    cnpj = (Int64)(dr["Cnpj"]);
                }
                return cnpj;
            }
        }

        public ClientePJ ObterCliente(int idCli)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbClientePJG where(idCli = @IdCli);", conexao);
                cmd.Parameters.AddWithValue("@IdCli", idCli);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                ClientePJ clientePJ = new ClientePJ();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    clientePJ.Nome = dr["Nome"].ToString();
                    clientePJ.Telefone = dr["Telefone"].ToString();
                    clientePJ.Email = dr["Email"].ToString();
                    clientePJ.Cep = dr["Cep"].ToString();
                    clientePJ.NumeroEndereco = Convert.ToInt32(dr["NumeroEndereco"]);
                    clientePJ.ComplementoEndereco = dr["ComplementoEndereco"] == DBNull.Value ? null : dr["ComplementoEndereco"].ToString();
                    clientePJ.Logradouro = dr["Logradouro"].ToString();
                    clientePJ.Bairro = dr["Bairro"].ToString();
                    clientePJ.Cidade = dr["Cidade"].ToString();
                    clientePJ.Estado = dr["Estado"].ToString();
                    clientePJ.Cnpj = dr["Cnpj"].ToString();
                }

                dr.Close();
                return clientePJ;
            }
        }
    }
}
