using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository.Contract;
using MySql.Data.MySqlClient;

namespace LivrariaLegitimo.Repository
{
    public class AvaliacaoRepository : IAvaliacaoRepository
    {
        public void ExisteAvaliacaoConta(int codBarras, int idCli)
        {
            using(var conexao = new MySqlConnection(_conexaoMySQL))
        }

        public void FazerAvaliacao(Avaliacao avaliacao, int idCli)
        {
            throw new NotImplementedException();
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
