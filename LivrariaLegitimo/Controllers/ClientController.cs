using Microsoft.AspNetCore.Mvc;

namespace LivrariaLegitimo.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
