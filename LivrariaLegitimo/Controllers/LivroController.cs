using LivrariaLegitimo.Libraries.Login;
using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository;
using LivrariaLegitimo.Repository.Contract;
using LivrariaLegitimo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using System.Diagnostics;

namespace LivrariaLegitimo.Controllers
{
    public class LivroController : Controller
    {
        private IClienteRepository _clienteRepository;
        private IFuncionarioRepository _funcionarioRepository;
        private LoginFuncionario _loginFuncionario;
        private LoginCliente _loginCliente;
        private ILivroRepository _livroRepository;
        public LivroController(IClienteRepository clienteRepository, IFuncionarioRepository funcionarioRepository, LoginFuncionario loginFuncionario, LoginCliente loginCliente, ILivroRepository livroRepository)
        {
            _funcionarioRepository = funcionarioRepository;
            _clienteRepository = clienteRepository;
            _loginFuncionario = loginFuncionario;
            _loginCliente = loginCliente;
            _livroRepository = livroRepository;
        }

        [HttpGet]
        public IActionResult Livro(string codBarras)
        {
            var livro = _livroRepository.ObterLivro(codBarras);

            var model = new LivroViewModel
            {
                ObterLivro = livro
            };

            return View(model);
        }
    }
}
