using LivrariaLegitimo.CarrinhoCompra;
using Microsoft.AspNetCore.Mvc;
using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository.Contract;

namespace LivrariaLegitimo.Controllers
{
    public class CarrinhoController : Controller
    {
        private ILivroRepository _livroRepository;
        private CookieCarrinhoCompra _cookieCarrinhoCompra;
        public CarrinhoController(CookieCarrinhoCompra cookieCarrinhoCompra, ILivroRepository livroRepository) { 
            _cookieCarrinhoCompra = cookieCarrinhoCompra;
            _livroRepository = livroRepository;
        }
        [HttpGet]
        public IActionResult Carrinho()
        {
            return View(_cookieCarrinhoCompra.Consultar());
        }
        public IActionResult AdicionarItem(string codBarras)
        {
            (Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas) livro = _livroRepository.ObterLivro(codBarras);

            if (livro.livro == null)
            {
                return NotFound();
            }

            var item = (livro.livro, livro.arquivoImagem, livro.arquivoEbook, livro.mediaLivro, livro.numeroVendas);

            _cookieCarrinhoCompra.Cadastrar(item);

            return RedirectToAction(nameof(Carrinho));
        }

        public IActionResult RemoverItem(string codBarras)
        {
            var Lista = _cookieCarrinhoCompra.Consultar();
            var ItemLocalizado = Lista.SingleOrDefault(a => a.livro.CodigoBarras.ToString() == codBarras);

            if (ItemLocalizado.livro != null)
            {
                _cookieCarrinhoCompra.Remover(ItemLocalizado.livro);
            }

            return RedirectToAction(nameof(Carrinho));
        }

        public IActionResult LimparCarrinho()
        {
            _cookieCarrinhoCompra.RemoverTodos();

            return RedirectToAction(nameof(Carrinho));
        }
    }
}
