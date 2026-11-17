
using LivrariaLegitimo.Cookie;
using LivrariaLegitimo.Models;
using Newtonsoft.Json;

namespace LivrariaLegitimo.CarrinhoCompra
{



    public class CookieCarrinhoCompra
    {
        private string Key = "Carrinho.Compras";
        private Cookie.Cookie _cookie;

        public CookieCarrinhoCompra(Cookie.Cookie cookie)
        {
            _cookie = cookie;
        }

        public void Salvar(List<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> Lista)
        {
            string Valor = JsonConvert.SerializeObject(Lista);
            _cookie.Cadastrar(Key, Valor);
        }

        public List<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> Consultar()
        {
            if (_cookie.Existe(Key))
            {
                string valor = _cookie.Consultar(Key);

                if (!string.IsNullOrEmpty(valor))
                {
                    var lista = JsonConvert.DeserializeObject<List<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)>>(valor);

                    return lista?.Where(a => a.livro != null).ToList()
                        ?? new List<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)>();
                }
            }

            return new List<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)>();
        }

        public void Cadastrar((Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas) item)
        {
            if (item.livro == null)
            {
                return;
            }

            List<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> Lista;

            if (_cookie.Existe(Key))
            {
                Lista = Consultar();

                var ItemLocalizado = Lista.SingleOrDefault(a => a.livro != null && a.livro.CodigoBarras == item.livro.CodigoBarras);

                if (ItemLocalizado.livro == null)
                {
                    Lista.Add(item);
                }
                else
                {
                    ItemLocalizado.livro.Quantidade = ItemLocalizado.livro.Quantidade + 1;
                }
            }
            else
            {
                Lista = new List<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)>();
                Lista.Add(item);
            }

            Salvar(Lista);
        }

        public void Atualizar(Livro item)
        {
            var Lista = Consultar();
            var ItemLocalizado = Lista.SingleOrDefault(a => a.livro.CodigoBarras == item.CodigoBarras);

            if (ItemLocalizado.livro != null)
            {
                ItemLocalizado.livro.Quantidade = item.Quantidade + 1;
                Salvar(Lista);
            }
        }

        public void Remover(Livro item)
        {
            if (item == null)
            {
                return;
            }

            var Lista = Consultar();

            var ItemLocalizado = Lista.SingleOrDefault(a =>
                a.livro != null &&
                a.livro.CodigoBarras == item.CodigoBarras);

            if (ItemLocalizado.livro != null)
            {
                Lista.Remove(ItemLocalizado);
                Salvar(Lista);
            }
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
