using LivrariaLegitimo.Historicos;
using LivrariaLegitimo.Libraries.Login;
using LivrariaLegitimo.Repository.Contract;
using Microsoft.AspNetCore.Mvc;
using LivrariaLegitimo.Models;
using LivrariaLegitimo.ViewModels;

namespace LivrariaLegitimo.Controllers
{
    public class ClienteController : Controller
    {
        private IClienteRepository _clienteRepository;
        private IClientePFRepository _clientePFRepository;
        private IClientePJRepository _clientePJRepository;
        private IFuncionarioRepository _funcionarioRepository;
        private LoginFuncionario _loginFuncionario;
        private LoginCliente _loginCliente;
        public ClienteController(IFuncionarioRepository funcionarioRepository, IClienteRepository clienteRepository,  LoginFuncionario loginFuncionario, LoginCliente loginCliente, IClientePFRepository clientePFRepository, IClientePJRepository clientePJRepository)
        {
            _funcionarioRepository = funcionarioRepository;
            _clienteRepository = clienteRepository;
            _loginFuncionario = loginFuncionario;
            _loginCliente = loginCliente;
            _clientePFRepository = clientePFRepository;
            _clientePJRepository = clientePJRepository;
        }


        [HttpGet]
        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(CadastroViewModel model)
        {
            if(model.TipoCliente == "PF")
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                _clientePFRepository.Cadastrar(model.CadastroPF);
            }
            else if(model.TipoCliente == "PJ")
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                _clientePJRepository.Cadastrar(model.CadastroPJ);
            }
                return RedirectToAction("Cadastro");
        }
    }
}
