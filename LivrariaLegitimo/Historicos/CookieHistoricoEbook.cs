
using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository.Contract;
using Newtonsoft.Json;
using System.Globalization;

namespace LivrariaLegitimo.Historicos
{
    public class CookieHistoricoEbook
    {
        private string Key = "Historico.Ebooks";
        private Cookie.Cookie _cookie;
        private ILivroRepository _livroRepository;

        public CookieHistoricoEbook(Cookie.Cookie cookie, ILivroRepository livroRepository)
        {
            _cookie = cookie;
            _livroRepository = livroRepository;
        }

        public void Salvar(Dictionary<string, int[]> Lista)
        {
            string Valor = JsonConvert.SerializeObject(Lista);
            _cookie.Cadastrar(Key, Valor);
        }

        // Recupera os ebooks completos e seus progressos
        public List<(Livro livro, string arquivoImagem, string arquivoEbook, int paginaAtual, int paginaTotal)> Consultar()
        {
            List<(Livro livro, string arquivoImagem, string arquivoEbook, int paginaAtual, int paginaTotal)> Lista = new();

            if (!_cookie.Existe(Key))
            {
                return Lista;
            }

            string valor = _cookie.Consultar(Key);

            if (string.IsNullOrEmpty(valor))
            {
                return Lista;
            }

            Dictionary<string, int[]> codigos =
                JsonConvert.DeserializeObject<Dictionary<string, int[]>>(valor)
                ?? new Dictionary<string, int[]>();

            foreach (var item in codigos)
            {
                var livro = _livroRepository.ObterLivro(item.Key);

                if (livro.livro != null && item.Value != null && item.Value.Length >= 2)
                {
                    Lista.Add((
                        livro.livro,
                        livro.arquivoImagem,
                        livro.arquivoEbook,
                        item.Value[0],
                        item.Value[1]
                    ));
                }
            }

            return Lista;
        }

        // Adiciona um ebook ao histórico ou atualiza sua posição
        public bool Cadastrar(string codBarras, int paginaAtual, int paginaTotal)
        {
            var livro = _livroRepository.ObterLivro(codBarras);

            if (livro.livro == null)
            {
                return false;
            }

            Dictionary<string, int[]> Lista = ConsultarCodigos();

            int[] progresso;

            if (Lista.ContainsKey(codBarras))
            {
                progresso = Lista[codBarras];
                Lista.Remove(codBarras);
            }
            else
            {
                progresso = new int[] { paginaAtual, paginaTotal };
            }

            Dictionary<string, int[]> NovaLista = new()
            {
                { codBarras, progresso }
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

        // Recupera os códigos e seus respectivos progressos
        private Dictionary<string, int[]> ConsultarCodigos()
        {
            if (!_cookie.Existe(Key))
            {
                return new Dictionary<string, int[]>();
            }

            string valor = _cookie.Consultar(Key);

            if (string.IsNullOrEmpty(valor))
            {
                return new Dictionary<string, int[]>();
            }

            return JsonConvert.DeserializeObject<Dictionary<string, int[]>>(valor)
                   ?? new Dictionary<string, int[]>();
        }

        // Atualiza a página atual e a quantidade total de páginas
        public bool AtualizarProgresso(string codBarras, int paginaAtual, int paginaTotal)
        {
            Dictionary<string, int[]> Lista = ConsultarCodigos();

            if (!Lista.ContainsKey(codBarras))
            {
                return false;
            }

            Lista[codBarras] = new int[] { paginaAtual, paginaTotal };

            Salvar(Lista);

            return true;
        }

        // Remove completamente um ebook do histórico
        public void Remover(Livro item)
        {
            if (item == null)
            {
                return;
            }

            Dictionary<string, int[]> Lista = ConsultarCodigos();

            string codigo = item.CodigoBarras.ToString(
                CultureInfo.InvariantCulture);

            Lista.Remove(codigo);

            Salvar(Lista);
        }

        // Remove todos os ebooks do histórico
        public void RemoverTodos()
        {
            _cookie.Remover(Key);
        }
    }
}
