using LivrariaLegitimo.CarrinhoCompra;
using LivrariaLegitimo.Libraries.Filtro;
using LivrariaLegitimo.Libraries.Login;
using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace LivrariaLegitimo.Areas.Worker.Controllers
{
    [Area("Worker")]
    public class FuncionarioController : Controller
    {
        private IFuncionarioRepository _funcionarioRepository;
        private LoginFuncionario _loginFuncionario;
        private LoginCliente _loginCliente;
        public FuncionarioController(IFuncionarioRepository funcionarioRepository, LoginCliente loginCliente, LoginFuncionario loginFuncionario)
        {
            _funcionarioRepository = funcionarioRepository;
            _loginCliente = loginCliente;
            _loginFuncionario = loginFuncionario;
        }
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

        [HttpGet]
        public IActionResult Dados()
        {
            Funcionario funcionario = _funcionarioRepository.ObterFuncionario(_loginFuncionario.GetFuncionario().IdFuncionario);
            return View(funcionario);
        }
    }
}
