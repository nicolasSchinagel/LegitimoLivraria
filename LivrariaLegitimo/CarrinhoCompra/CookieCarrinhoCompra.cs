
using LivrariaLegitimo.Cookie;
using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository.Contract;
using Newtonsoft.Json;
using System.Globalization;

namespace LivrariaLegitimo.CarrinhoCompra
{
    public class CookieCarrinhoCompra
    {
        private string Key = "Carrinho.Compras";
        private Cookie.Cookie _cookie;
        private ILivroRepository _livroRepository;

        public CookieCarrinhoCompra(Cookie.Cookie cookie, ILivroRepository livroRepository)
        {
            _cookie = cookie;
            _livroRepository = livroRepository;
        }

        // Salva os códigos de barras e suas respectivas quantidades
        public void Salvar(Dictionary<string, int> Lista)
        {
            string Valor = JsonConvert.SerializeObject(Lista);
            _cookie.Cadastrar(Key, Valor);
        }

        // Recupera os livros completos do banco e suas quantidades no carrinho
        public List<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas, int quantidadeCarrinho)> Consultar()
        {
            List<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas, int quantidadeCarrinho)> Lista = new();

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
                        livro.numeroVendas,
                        item.Value
                    ));
                }
            }

            return Lista;
        }

        // Adiciona um livro ou incrementa sua quantidade
        public bool Cadastrar(string codBarras)
        {
            var livro = _livroRepository.ObterLivro(codBarras);

            if (livro.livro == null)
            {
                return false;
            }

            Dictionary<string, int> Lista = ConsultarCodigos();

            if (Lista.ContainsKey(codBarras) && livro.livro.TipoCapa == "ebook")
            {
                Lista[codBarras]++;
            }
            else
            {
                Lista.Add(codBarras, 1);
            }

            Salvar(Lista);

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

        public bool AumentarQuantidade(string codBarras)
        {
            Dictionary<string, int> Lista = ConsultarCodigos();

            if (!Lista.ContainsKey(codBarras))
            {
                return false;
            }

            Lista[codBarras]++;

            Salvar(Lista);

            return true;
        }

        public bool DiminuirQuantidade(string codBarras)
        {
            Dictionary<string, int> Lista = ConsultarCodigos();

            if (!Lista.ContainsKey(codBarras))
            {
                return false;
            }

            if (Lista[codBarras] > 1)
            {
                Lista[codBarras]--;
            }
            else
            {
                Lista.Remove(codBarras);
            }

            Salvar(Lista);

            return true;
        }
    }
}
