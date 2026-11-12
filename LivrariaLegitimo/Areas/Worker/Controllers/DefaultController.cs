using Microsoft.AspNetCore.Mvc;

namespace LivrariaLegitimo.Areas.Worker.Controllers
{
    [Area("Worker")]
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MenuGerenciar()
        {
            return View();
        }
    }
}
