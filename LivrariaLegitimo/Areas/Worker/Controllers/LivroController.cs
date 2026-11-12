using LivrariaLegitimo.Libraries.Filtro;
using LivrariaLegitimo.Repository.Contract;
using LivrariaLegitimo.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LivrariaLegitimo.Areas.Worker.Controllers
{
    [Area("Worker")]
    public class LivroController : Controller
    {
        private ILivroRepository _livroRepository;
        public LivroController(ILivroRepository livroRepository)
        {
            _livroRepository = livroRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [FuncionarioAutorizacao("administrador total", "gerente de compras", "gerente de contratação", "gerente comercial", "caixa")]
        public IActionResult AdicionarLivro()
        {
            return View();
        }
    }
}
