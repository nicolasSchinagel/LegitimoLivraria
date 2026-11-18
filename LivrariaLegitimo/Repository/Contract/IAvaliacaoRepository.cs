using LivrariaLegitimo.Models;
using X.PagedList;

namespace LivrariaLegitimo.Repository.Contract
{
    public interface IAvaliacaoRepository
    {
        void FazerAvaliacao(Avaliacao avaliacao, int idCli);

        // void RemoverAvaliacao(int codBarras, int Id);     Ainda estou vendo
        bool ExisteAvaliacaoConta(int codBarras, int Id);
        IEnumerable<(Avaliacao avaliacao, string NomeCliente)> ObterTodasAvaliacoes(string codigoBarras);

        public (int quantidadeG, int nota0, int nota1, int nota2, int nota3, int nota4, int nota5) ObterQuantidadesAvaliacaoLivro(int idCli);
    }
}
