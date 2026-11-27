using LivrariaLegitimo.Models;

namespace LivrariaLegitimo.ViewModels
{
    public class LivroViewModel
    {
        public IEnumerable<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> MaisVendidos { get; set; }

        public IEnumerable<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> BemAvaliados { get; set; }

        public IEnumerable<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> Aleatorios { get; set; }

        public IEnumerable<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> MaioresOfertas { get; set; }

        public IEnumerable<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> TodosLivros { get; set; }

        public IEnumerable<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> HistoricoG { get; set; }

        public IEnumerable<(Livro livro, string arquivoImagem, string arquivoEbook, int paginaAtual, int paginaTotal)> HistoricoE { get; set; }

        public (Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)? ObterLivro { get; set; }

        public bool ExisteAvaliacaoConta { get; set; }

        public (int quantidadeG, int nota0, int nota1, int nota2, int nota3, int nota4, int nota5) ObterQuantidadesAvaliacaoLivro { get; set; }

        public Avaliacao AvaliacaoLivro { get; set; }
        public IEnumerable<(Avaliacao avaliacao, string NomeCliente)> ObterTodasAvaliacoes { get; set; }

        public List<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas, int quantidadeCarrinho)> CookieReposicao { get; set; }
    }
}
