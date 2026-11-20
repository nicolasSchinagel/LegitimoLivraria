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
        private IAvaliacaoRepository _avaliacaoRepository;
        public LivroController(IClienteRepository clienteRepository, IFuncionarioRepository funcionarioRepository, LoginFuncionario loginFuncionario, LoginCliente loginCliente, ILivroRepository livroRepository, IAvaliacaoRepository avaliacaoRepository)
        {
            _funcionarioRepository = funcionarioRepository;
            _clienteRepository = clienteRepository;
            _loginFuncionario = loginFuncionario;
            _loginCliente = loginCliente;
            _livroRepository = livroRepository;
            _avaliacaoRepository = avaliacaoRepository;
        }

        [HttpGet]
        public IActionResult Livro(string codBarras)
        {
            var livro = _livroRepository.ObterLivro(codBarras);
            var quantidades = _avaliacaoRepository.ObterQuantidadesAvaliacaoLivro(codBarras);
            var avaliacoes = _avaliacaoRepository.ObterTodasAvaliacoes(codBarras);
            var existeAvaliacao = _avaliacaoRepository.ExisteAvaliacaoConta(codBarras);
            var aleatorios = _livroRepository.AleatorizarLivros();
            var emAlta = _livroRepository.EmAlta(codBarras);

            var model = new LivroViewModel
            {
                ObterLivro = livro,
                ObterQuantidadesAvaliacaoLivro = quantidades,
                ObterTodasAvaliacoes = avaliacoes,
                ExisteAvaliacaoConta = existeAvaliacao,
                Aleatorios = aleatorios,
                EmAlta = emAlta
            };

            return View(model);
        }


    }
}
