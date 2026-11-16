using Microsoft.AspNetCore.Mvc;

namespace LivrariaLegitimo.Controllers
{
    public class CarrinhoController : Controller
    {
        public IActionResult Carrinho()
        {
            return View();
        }
    }
}
