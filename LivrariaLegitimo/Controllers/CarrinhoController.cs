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
            return View();
        }
    }
}
