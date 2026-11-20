using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository.Contract;
using Newtonsoft.Json;
using System.Globalization;

namespace LivrariaLegitimo.Historicos
{
    public class CookieHistoricoLivro
    {
        private string Key = "Historico.Livros";
        private Cookie.Cookie _cookie;
        private ILivroRepository _livroRepository;
        public CookieHistoricoLivro(Cookie.Cookie cookie, ILivroRepository livroRepository)
        {
            _cookie = cookie;
            _livroRepository = livroRepository;
        }
        public void Salvar(Dictionary<string, int> Lista)
        {
            string Valor = JsonConvert.SerializeObject(Lista);
            _cookie.Cadastrar(Key, Valor);
        }

        // Recupera os livros completos do banco e suas quantidades no carrinho
        public List<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> Consultar()
        {
            List<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> Lista = new();

            if (!_cookie.Existe(Key))
            {
                return Lista;
            }

            string valor = _cookie.Consultar(Key);

            if (string.IsNullOrEmpty(valor))
            {
                return Lista;
            }

            Dictionary<string, int> codigos =
                JsonConvert.DeserializeObject<Dictionary<string, int>>(valor)
                ?? new Dictionary<string, int>();

            foreach (var item in codigos)
            {
                var livro = _livroRepository.ObterLivro(item.Key);

                if (livro.livro != null)
                {
                    Lista.Add((
                        livro.livro,
                        livro.arquivoImagem,
                        livro.arquivoEbook,
                        livro.mediaLivro,
                        livro.numeroVendas
                    ));
                }
            }

            return Lista;
        }

        // Adiciona um livro ou incrementa sua quantidade
        // Adiciona um livro ao histórico ou atualiza sua posição
        public bool Cadastrar(string codBarras)
        {
            var livro = _livroRepository.ObterLivro(codBarras);

            if (livro.livro == null)
            {
                return false;
            }

            Dictionary<string, int> Lista = ConsultarCodigos();

            // Remove o livro caso já esteja no histórico
            Lista.Remove(codBarras);

            // Cria um novo histórico colocando o livro no início
            Dictionary<string, int> NovaLista = new Dictionary<string, int>
            {
                { codBarras, 1 }
            };

            foreach (var item in Lista)
            {
                if (NovaLista.Count >= 15)
                {
                    break;
                }

                NovaLista.Add(item.Key, item.Value);
            }

            Salvar(NovaLista);

            return true;
        }

        // Recupera os códigos e suas quantidades
        private Dictionary<string, int> ConsultarCodigos()
        {
            if (!_cookie.Existe(Key))
            {
                return new Dictionary<string, int>();
            }

            string valor = _cookie.Consultar(Key);

            if (string.IsNullOrEmpty(valor))
            {
                return new Dictionary<string, int>();
            }

            return JsonConvert.DeserializeObject<Dictionary<string, int>>(valor)
                   ?? new Dictionary<string, int>();
        }

        // Remove completamente um livro do carrinho
        public void Remover(Livro item)
        {
            if (item == null)
            {
                return;
            }

            Dictionary<string, int> Lista = ConsultarCodigos();

            string codigo = item.CodigoBarras.ToString(
                CultureInfo.InvariantCulture);

            Lista.Remove(codigo);

            Salvar(Lista);
        }

        // Verifica se o cookie existe
        public bool Existe(string Key)
        {
            return _cookie.Existe(Key);
        }

        // Remove todos os livros do carrinho
        public void RemoverTodos()
        {
            _cookie.Remover(Key);
        }

    }
}
