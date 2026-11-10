using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository.Contract;
using MySql.Data.MySqlClient;
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
        public void Atualizar(ClientePJ clientePJ)
        {
            throw new NotImplementedException();
        }

        public void Cadastrar(ClientePJ clientePJ)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("call spCadastroClientePJ(@Nome, @CNPJ , @Email, @Telefone , @Senha , @CEP, @NumeroEndereco, @Complemento, @Logradouro, @Bairro, @Cidade, @Estado );", conexao);

                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = clientePJ.Nome;
                cmd.Parameters.Add("@CNPJ", MySqlDbType.Int32).Value = Convert.ToInt32(clientePJ.Cnpj.Replace("-", "").Replace(".", "").Replace("/", ""));
                cmd.Parameters.Add("@Telefone", MySqlDbType.Int32).Value = Convert.ToInt32(clientePJ.Telefone.Replace("(", "").Replace(")", "").Replace("-", ""));
                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = clientePJ.Email;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = clientePJ.Senha;
                cmd.Parameters.Add("@CEP", MySqlDbType.Decimal).Value = Convert.ToDecimal(clientePJ.Cep.Replace("-", ""));
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

    }
}
