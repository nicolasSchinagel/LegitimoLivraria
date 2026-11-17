
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
                return JsonConvert.DeserializeObject<List<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)>>(valor);
            }
            else
            {
                return new List<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)>();
            }
        }

        public void Cadastrar((Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas) item)
        {
            List<(Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> Lista;

            if (_cookie.Existe(Key))
            {
                Lista = Consultar();
                var ItemLocalizado = Lista.SingleOrDefault(a => a.livro.CodigoBarras == item.livro.CodigoBarras);

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
            var Lista = Consultar();
            var ItemLocalizado = Lista.SingleOrDefault(a => a.livro.CodigoBarras == item.CodigoBarras);

            if (ItemLocalizado.livro != null)
            {
                Lista.Remove(ItemLocalizado);
                Salvar(Lista);
            }
        }

        public bool Existe(string Key)
        {
            if (_cookie.Existe(Key))
            {
                return false;
            }
            return true;
        }

        public void RemoverTodos()
        {
            _cookie.Remover(Key);
        }
    }

}
