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
        public bool ExisteAvaliacaoConta(string codBarras)
        {
            if (_loginCliente.GetCliente() != null)
            {
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    bool existencia;
                    conexao.Open();
                    MySqlCommand cmd = new MySqlCommand("select * from tbAvaliacao where (codBarras = @CodBarras and idCli = @IdCli)", conexao);
                    cmd.Parameters.AddWithValue("@CodBarras", Convert.ToDecimal(codBarras));
                    cmd.Parameters.AddWithValue("@IdCli", _loginCliente.GetCliente().IdCli);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    MySqlDataReader dr;

                    Avaliacao avaliacao = new Avaliacao();
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    existencia = dr.Read();
                    return existencia;
                }
            }
            else
            {
                return false;
            }
        }

        public void FazerAvaliacao(Avaliacao avaliacao, int idCli)
        {
            using(var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("call spFazerAvaliacao(@CodBarras, @IdCli, @NotaLivro, @Comentario, @TituloComentario);", conexao);
                cmd.Parameters.Add("@CodBarras", MySqlDbType.Decimal).Value = avaliacao.CodigoBarras;
                cmd.Parameters.Add("@IdCli", MySqlDbType.Int32).Value = idCli;
                cmd.Parameters.Add("@NotaLivro", MySqlDbType.Int32).Value = avaliacao.NotaLivro;
                cmd.Parameters.Add("@Comentario", MySqlDbType.VarChar).Value = avaliacao.Comentario;
                cmd.Parameters.Add("@TituloComentario", MySqlDbType.VarChar).Value = avaliacao.TituloAvaliacao;

                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public (int quantidadeG, int nota0,  int nota1, int nota2, int nota3, int nota4, int nota5) ObterQuantidadesAvaliacaoLivro(string codBarras)
        {
            using(var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from ViewLivroQuantidadeAvaliacoes where codBarras = @CodigoBarras;", conexao);
                cmd.Parameters.AddWithValue("@CodigoBarras", Convert.ToDecimal(codBarras));

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                (int quantidadeG, int nota0, int nota1, int nota2, int nota3, int nota4, int nota5) quantidades = (0, 0, 0, 0, 0, 0, 0);
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    quantidades.quantidadeG = Convert.ToInt32(dr["QuantidadeTotal"]);
                    quantidades.nota0 = Convert.ToInt32(dr["Nota0"]);
                    quantidades.nota1 = Convert.ToInt32(dr["Nota1"]);
                    quantidades.nota2 = Convert.ToInt32(dr["Nota2"]);
                    quantidades.nota3 = Convert.ToInt32(dr["Nota3"]);
                    quantidades.nota4 = Convert.ToInt32(dr["Nota4"]);
                    quantidades.nota5 = Convert.ToInt32(dr["Nota5"]);
                }


                return quantidades;
            }
        }

        public IEnumerable<(Avaliacao avaliacao, string NomeCliente)> ObterTodasAvaliacoes(string codigoBarras)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand(
                    "select * from ViewAvaliacao where codBarras = @CodigoBarras;",
                    conexao
                );

                cmd.Parameters.AddWithValue("@CodigoBarras", Convert.ToDecimal(codigoBarras));

                MySqlDataReader dr = cmd.ExecuteReader();

                List<(Avaliacao avaliacao, string NomeCliente)> avaliacoes = new();

                while (dr.Read())
                {
                    Avaliacao avaliacao = new Avaliacao();

                    avaliacao.CodigoBarras = (decimal)dr["codBarras"];
                    avaliacao.NotaLivro = (Int16)dr["nota"];
                    avaliacao.Comentario = (string)dr["comentario"];
                    avaliacao.TituloAvaliacao = (string)dr["titulo"];
                    avaliacao.DataAvaliacao = DateOnly.FromDateTime(Convert.ToDateTime(dr["dataAvaliacao"]));

                    string NomeCliente = (string)dr["nome"];

                    avaliacoes.Add((avaliacao, NomeCliente));
                }

                dr.Close();

                return avaliacoes;
            }
        }
    }
}
