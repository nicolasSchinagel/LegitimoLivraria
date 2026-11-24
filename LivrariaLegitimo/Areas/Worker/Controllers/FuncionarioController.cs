using LivrariaLegitimo.CarrinhoCompra;
using LivrariaLegitimo.Libraries.Filtro;
using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace LivrariaLegitimo.Areas.Worker.Controllers
{
    public class FuncionarioController : Controller
    {
        private IFuncionarioRepository _funcionarioRepository;
        public FuncionarioController(IFuncionarioRepository funcionarioRepository)
        {
            _funcionarioRepository = funcionarioRepository;
        }
        [Area("Worker")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [FuncionarioAutorizacao("administrador total", "gerente de contratação")]
        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        [FuncionarioAutorizacao("administrador total", "gerente de contratação")]
        public IActionResult Cadastro(Funcionario funcionario)
        {
            _funcionarioRepository.Cadastrar(funcionario);
            TempData["msg"] = "Cadastro realizado!";
            return RedirectToAction("MenuGerenciar", "Default");
        }
    }
}
