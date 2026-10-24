using LivrariaLegitimo.Models;
using X.PagedList;

namespace LivrariaLegitimo.Repository.Contract
{
    public interface IAvaliacaoRepository
    {
        void FazerAvaliacao(Avaliacao avaliacao);
        void RemoverAvaliacao(int codBarras, int Id);
        void ExisteAvaliacaoConta(int codBarras, int Id);
        IEnumerable<Avaliacao> ObterTodasAvaliacoes();
        IPagedList<Avaliacao> ObterTodasAvaliacoes(int? pagina, string pesquisa);
    }
}
