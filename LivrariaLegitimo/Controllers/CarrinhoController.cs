using LivrariaLegitimo.CarrinhoCompra;
using Microsoft.AspNetCore.Mvc;

namespace LivrariaLegitimo.Controllers
{
    public class CarrinhoController : Controller
    {
        private CookieCarrinhoCompra _cookieCarrinhoCompra;
        public CarrinhoController(CookieCarrinhoCompra cookieCarrinhoCompra) { 
            _cookieCarrinhoCompra = cookieCarrinhoCompra;
        }
        public IActionResult Carrinho()
        {
            return View(_cookieCarrinhoCompra.Consultar());
        }
        public IActionResult AdicionarItem(string codBarras)
        {

        }
        public IActionResult RemoverItem(string codBarras)
        {

        }
    }
}
