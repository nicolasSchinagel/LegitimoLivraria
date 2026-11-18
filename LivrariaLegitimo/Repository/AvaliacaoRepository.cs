using LivrariaLegitimo.Libraries.Login;
using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository.Contract;
using MySql.Data.MySqlClient;
using System.Data;
using System.Globalization;

namespace LivrariaLegitimo.Repository
{
    public class AvaliacaoRepository : IAvaliacaoRepository
    {
        private readonly string _conexaoMySQL;
        IConfiguration _config;
        private readonly LoginCliente _loginCliente;
        private readonly LoginFuncionario _loginFuncionario;
        public AvaliacaoRepository(LoginCliente loginCliente, LoginFuncionario loginFuncionario, IConfiguration conf)
        {
            _loginCliente = loginCliente;
            _loginFuncionario = loginFuncionario;
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
            _config = conf;
        }
        public bool ExisteAvaliacaoConta(int codBarras, int idCli)
        {
            using(var conexao = new MySqlConnection(_conexaoMySQL))
            {
                bool existencia;
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbAvaliacao where (codBarras = @CodBarras and idCli = @IdCli");
                cmd.Parameters.AddWithValue("@CodBarras", codBarras);
                cmd.Parameters.AddWithValue("@IdCli", idCli);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Avaliacao avaliacao = new Avaliacao();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                
                existencia = dr.Read();
                return existencia;
            }
        }

        public void FazerAvaliacao(Avaliacao avaliacao, int idCli)
        {
            using(var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("call spFazerAvaliacao(@CodBarras, @IdCli, @NotaLivro, @Comentario, @TituloComentario);");
                cmd.Parameters.Add("@CodBarras", MySqlDbType.Decimal).Value = avaliacao.CodigoBarras;
                cmd.Parameters.Add("@IdCli", MySqlDbType.Int32).Value = idCli;
                cmd.Parameters.Add("@NotaLivro", MySqlDbType.Int32).Value = avaliacao.NotaLivro;
                cmd.Parameters.Add("@Comentario", MySqlDbType.VarChar).Value = avaliacao.Comentario;
                cmd.Parameters.Add("@TituloComentario", MySqlDbType.VarChar).Value = avaliacao.TituloAvaliacao;

                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public (int quantidadeG, int nota1, int nota2, int nota3, int nota4, int nota5) ObterQuantidadesAvaliacaoLivro(int idCli)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Avaliacao> ObterTodasAvaliacoes()
        {
            throw new NotImplementedException();
        }
    }
}
