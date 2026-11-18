using LivrariaLegitimo.Models;

namespace LivrariaLegitimo.ViewModels
{
    public class LivroViewModel
    {
        public IEnumerable<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> MaisVendidos { get; set; }

        public IEnumerable<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> BemAvaliados { get; set; }

        public IEnumerable<(Livro livro, string arquivoImagem, string arquivoEbook)> Aleatorios { get; set; }

        public IEnumerable<(Livro livro, string arquivoImagem, string arquivoEbook)> MaioresOfertas { get; set; }

        public IEnumerable<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> TodosLivros { get; set; }

        public IEnumerable<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> HistoricoG { get; set; }

        public IEnumerable<(Livro livro, string arquivoImagem, string arquivoEbook)> HistoricoE { get; set; }

        public (Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas) ObterLivro { get; set; }

        public bool ExisteAvaliacaoConta { get; set; }

        public (int quantidadeG, int nota0, int nota1, int nota2, int nota3, int nota4, int nota5) ObterQuantidadesAvaliacaoLivro { get; set; }

        public IEnumerable<(Avaliacao avaliacao, string NomeCliente)> ObterTodasAvaliacoes { get; set; }


    }
}
