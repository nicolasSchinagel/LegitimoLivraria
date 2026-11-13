using LivrariaLegitimo.Libraries.Filtro;
using LivrariaLegitimo.Repository.Contract;
using LivrariaLegitimo.Models;
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
        [FuncionarioAutorizacao("administrador total", "gerente de compras")]
        public IActionResult AdicionarLivro()
        {
            return View();
        }


        [HttpPost]
        [FuncionarioAutorizacao("administrador total", "gerente de compras")]
        public IActionResult AdicionarLivro(Livro livro)
        {
            _livroRepository.Cadastrar(livro);
            TempData["msg"] = "Cadastro realizado!";
            return RedirectToAction("MenuGerenciar", "Default");
        }
        
    }
}
