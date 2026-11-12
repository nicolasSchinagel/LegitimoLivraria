using Microsoft.AspNetCore.Mvc;
using LivrariaLegitimo.Libraries.Filtro;
namespace LivrariaLegitimo.Areas.Worker.Controllers
{
    [Area("Worker")]
    public class DefaultController : Controller
    {
        [FuncionarioAutorizacao("administrador total", "gerente de compras", "gerente de contratação", "gerente comercial", "caixa")]
        public IActionResult Index()
        {
            return View();
        }
        [FuncionarioAutorizacao("administrador total", "gerente de compras", "gerente de contratação", "gerente comercial", "caixa")]
        public IActionResult MenuGerenciar()
        {
            return View();
        }
    }
}
