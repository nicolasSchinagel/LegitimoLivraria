

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

        // Salva somente os códigos de barras
        public void Salvar(List<string> Lista)
        {
            string Valor = JsonConvert.SerializeObject(Lista);
            _cookie.Cadastrar(Key, Valor);
        }

        // Recupera os livros completos do banco
        public List<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> Consultar()
        {
            List<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> Lista = new();


            if (!_cookie.Existe(Key))
            {
                return Lista;
            }

            string valor = _cookie.Consultar(Key);

            Console.WriteLine("VALOR RECEBIDO: " + valor);
            Console.WriteLine("TIPO ESPERADO: List<string>");
            if (string.IsNullOrEmpty(valor))
            {
                return Lista;
            }

            List<string> codigos =
                JsonConvert.DeserializeObject<List<string>>(valor)
                ?? new List<string>();

            foreach (var codigo in codigos)
            {
                var livro = _livroRepository.ObterLivro(codigo);

                if (livro.livro != null)
                {
                    Lista.Add(livro);
                }
            }

            return Lista;
        }

        // Adiciona um código de barras
        public bool Cadastrar(string codBarras)
        {
            var livro = _livroRepository.ObterLivro(codBarras);

            if (livro.livro == null)
            {
                return false;
            }

            List<string> Lista = ConsultarCodigos();

            if (!Lista.Contains(codBarras))
            {
                Lista.Add(codBarras);
            }

            Salvar(Lista);

            return true;
        }

        // Recupera somente os códigos armazenados
        private List<string> ConsultarCodigos()
        {
            if (!_cookie.Existe(Key))
            {
                return new List<string>();
            }

            string valor = _cookie.Consultar(Key);

            if (string.IsNullOrEmpty(valor))
            {
                return new List<string>();
            }

            return JsonConvert.DeserializeObject<List<string>>(valor)
                   ?? new List<string>();
        }

        public void Remover(Livro item)
        {
            if (item == null)
            {
                return;
            }

            List<string> Lista = ConsultarCodigos();

            string codigo = item.CodigoBarras.ToString(
                CultureInfo.InvariantCulture);

            Lista.Remove(codigo);

            Salvar(Lista);
        }

        public bool Existe(string Key)
        {
            return _cookie.Existe(Key);
        }

        public void RemoverTodos()
        {
            _cookie.Remover(Key);
        }
    }
}
