using Microsoft.AspNetCore.Mvc;

namespace LivrariaLegitimo.Controllers
{
    public class LivroController : Controller
    {
        public IActionResult Livro()
        {
            return View();
        }
    }
}
