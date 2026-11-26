using LivrariaLegitimo.CarrinhoCompra;
using LivrariaLegitimo.Libraries.Filtro;
using LivrariaLegitimo.Libraries.Login;
using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository.Contract;
using Microsoft.AspNetCore.Mvc;
using LivrariaLegitimo.ViewModels;

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
        [HttpGet]
        [FuncionarioAutorizacao("administrador total")]
        public IActionResult Index()
        {
            IEnumerable<Funcionario> funcionarios = _funcionarioRepository.ObterTodosFuncionarios();
            return View(funcionarios);
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

        [HttpGet]
        [FuncionarioAutorizacao("administrador total")]
        public IActionResult Atualizar(int idFunc)
        {
            Funcionario funcionario = _funcionarioRepository.ObterFuncionario(idFunc);

            if (funcionario == null)
            {
                return NotFound();
            }

            FuncionarioEditarViewModel viewModel = new FuncionarioEditarViewModel
            {
                IdFuncionario = idFunc,
                Email = funcionario.Email,
                Telefone = funcionario.Telefone.ToString(),
                Cargo = funcionario.Cargo,
                Cep = funcionario.Cep,
                Logradouro = funcionario.Logradouro,
                Bairro = funcionario.Bairro,
                Cidade = funcionario.Cidade,
                Estado = funcionario.Estado,
                NumeroEndereco = funcionario.NumeroEndereco,
                ComplementoEndereco = funcionario.ComplementoEndereco,
                FimContrato = funcionario.FimContrato
            };

            return View(viewModel);
        }

        [HttpPost]
        [FuncionarioAutorizacao("administrador total")]
        public IActionResult Atualizar(FuncionarioEditarViewModel funcionario)
        {
            if (!ModelState.IsValid)
            {
                return View(funcionario);
            }

            _funcionarioRepository.Atualizar(funcionario);

            TempData["msg"] = "Cadastro feito!";
            return RedirectToAction("MenuGerenciar", "Default");
        }
    }
}
