using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository.Contract;
using MySql.Data.MySqlClient;
using X.PagedList;
namespace LivrariaLegitimo.Repository
{
    public class FuncionarioRepository : IFuncionarioRepository
    {
        private readonly string _conexaoMySQL;
        IConfiguration _config;
        public FuncionarioRepository(IConfiguration conf)
        {
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
            _config = conf;
        }
        public void Atualizar(Funcionario funcionario)
        {
            throw new NotImplementedException();
        }

        public void Cadastrar(Funcionario funcionario)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("call spCadastroFuncionario(@Nome, @CPF , @Telefone, @Email, @Cargo, @DataAdmissao, @FimContrato, @CEP, @Estado, @Cidade, @Bairro, @Logradouro, @Senha, @NumeroEndereco, @Complemento );", conexao);

                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = funcionario.Nome;
                cmd.Parameters.Add("@DataAdmissao", MySqlDbType.DateTime).Value = funcionario.DataAdmissao.ToString("yyyy/MM/dd");
                cmd.Parameters.Add("@FimContrato", MySqlDbType.Date).Value = funcionario.FimContrato.ToString("yyyy/MM/dd");
                cmd.Parameters.Add("@CPF", MySqlDbType.Int32).Value = Convert.ToInt32(funcionario.Cpf.Replace("-", "").Replace(".", ""));
                cmd.Parameters.Add("@Telefone", MySqlDbType.Int32).Value = Convert.ToInt32(funcionario.Telefone.Replace("(", "").Replace(")", "").Replace("-", ""));
                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = funcionario.Email;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = funcionario.Senha;
                cmd.Parameters.Add("@CEP", MySqlDbType.Decimal).Value = Convert.ToDecimal(funcionario.Cep.Replace("-", ""));
                cmd.Parameters.Add("@NumeroEndereco", MySqlDbType.Int32).Value = funcionario.NumeroEndereco;
                cmd.Parameters.Add("@Complemento", MySqlDbType.VarChar).Value = funcionario.ComplementoEndereco;
                cmd.Parameters.Add("@Logradouro", MySqlDbType.VarChar).Value = funcionario.Logradouro;
                cmd.Parameters.Add("@Bairro", MySqlDbType.VarChar).Value = funcionario.Bairro;
                cmd.Parameters.Add("@Cidade", MySqlDbType.VarChar).Value = funcionario.Cidade;
                cmd.Parameters.Add("@Estado", MySqlDbType.VarChar).Value = funcionario.Estado;
                cmd.Parameters.Add("@Cargo", MySqlDbType.VarChar).Value = funcionario.Cargo;
                    


                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public Funcionario Login(string Email, string Senha)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from ViewFuncionario where Email = @Email and Senha = @Senha", conexao);

                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = Email;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = Senha;

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Funcionario funcionario = new Funcionario();
                dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    funcionario.IdFuncionario = Convert.ToInt32(dr["idfuncionario"]);
                    funcionario.Nome = Convert.ToString(dr["Nome"]);
                    funcionario.Email = Convert.ToString(dr["Email"]);
                    funcionario.Senha = Convert.ToString(dr["Senha"]);
                    funcionario.Telefone = Convert.ToString(dr["NumTel"]);
                    funcionario.Cpf = Convert.ToString(dr["Cpf"]);
                    funcionario.Cargo = Convert.ToString(dr["Cargo"]);
                    funcionario.DataAdmissao = Convert.ToDateTime(dr["DataAdmissao"]);
                    funcionario.Cep = Convert.ToString(dr["Cep"]);
                    funcionario.Logradouro = Convert.ToString(dr["Logradouro"]);
                    funcionario.Bairro = Convert.ToString(dr["Bairro"]);
                    funcionario.Cidade = Convert.ToString(dr["Cidade"]);
                    funcionario.Estado = Convert.ToString(dr["Estado"]);
                    funcionario.NumeroEndereco = Convert.ToInt32(dr["NumeroEndereco"]);
                    funcionario.ComplementoEndereco = Convert.ToString(dr["ComplementoEndereco"]);
                    funcionario.FimContrato = Convert.ToDateTime(dr["FimContrato"]);
                }
                return funcionario;
            }
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
