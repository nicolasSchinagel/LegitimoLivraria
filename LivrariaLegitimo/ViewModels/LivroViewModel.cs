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
    }
}
