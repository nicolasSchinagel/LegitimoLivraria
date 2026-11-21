using LivrariaLegitimo.Historicos;
using LivrariaLegitimo.Libraries.Login;
using LivrariaLegitimo.Repository.Contract;
using Microsoft.AspNetCore.Mvc;
using LivrariaLegitimo.Models;

namespace LivrariaLegitimo.Controllers
{
    public class ClienteController : Controller
    {
        private IClienteRepository _clienteRepository;
        private IFuncionarioRepository _funcionarioRepository;
        private LoginFuncionario _loginFuncionario;
        private LoginCliente _loginCliente;
        public ClienteController(IFuncionarioRepository funcionarioRepository, IClienteRepository clienteRepository, LoginFuncionario loginFuncionario, LoginCliente loginCliente)
        {
            _funcionarioRepository = funcionarioRepository;
            _clienteRepository = clienteRepository;
            _loginFuncionario = loginFuncionario;
            _loginCliente = loginCliente;
        }


        [HttpGet]
        public IActionResult Cadastro()
        {
            return View();
        }
        /*
        [HttpPost]
        public IActionResult Cadastro(Cliente cliente)
        {

        }
        */
    }
}
