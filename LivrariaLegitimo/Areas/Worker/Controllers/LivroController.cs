using Microsoft.AspNetCore.Mvc;

namespace LivrariaLegitimo.Areas.Worker.Controllers
{
    public class LivroController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
