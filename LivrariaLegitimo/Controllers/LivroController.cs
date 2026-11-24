using LivrariaLegitimo.CarrinhoCompra;
using LivrariaLegitimo.Libraries.Login;
using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository;
using LivrariaLegitimo.Historicos;
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
        private CookieHistoricoLivro _cookieHistoricoLivro;
        public LivroController(CookieHistoricoLivro cookieHistoricoLivro, IClienteRepository clienteRepository, IFuncionarioRepository funcionarioRepository, LoginFuncionario loginFuncionario, LoginCliente loginCliente, ILivroRepository livroRepository, IAvaliacaoRepository avaliacaoRepository)
        {
            _funcionarioRepository = funcionarioRepository;
            _clienteRepository = clienteRepository;
            _loginFuncionario = loginFuncionario;
            _loginCliente = loginCliente;
            _livroRepository = livroRepository;
            _avaliacaoRepository = avaliacaoRepository;
            _cookieHistoricoLivro = cookieHistoricoLivro;
        }

        [HttpGet]
        public IActionResult Livro(string codBarras)
        {
            var livro = _livroRepository.ObterLivro(codBarras);
            var quantidades = _avaliacaoRepository.ObterQuantidadesAvaliacaoLivro(codBarras);
            var avaliacoes = _avaliacaoRepository.ObterTodasAvaliacoes(codBarras);
            var existeAvaliacao = _avaliacaoRepository.ExisteAvaliacaoConta(codBarras);
            var aleatorios = _livroRepository.AleatorizarLivros(codBarras);

            var model = new LivroViewModel
            {
                ObterLivro = livro,
                ObterQuantidadesAvaliacaoLivro = quantidades,
                ObterTodasAvaliacoes = avaliacoes,
                ExisteAvaliacaoConta = existeAvaliacao,
                Aleatorios = aleatorios,
            };

            _cookieHistoricoLivro.Cadastrar(codBarras);

            return View(model);
        }

        [HttpPost]
        public IActionResult Livro(LivroViewModel model)
        {
            _avaliacaoRepository.FazerAvaliacao(model.AvaliacaoLivro, _loginCliente.GetCliente().IdCli);

            return RedirectToAction("Livro", new { codBarras = model.AvaliacaoLivro.CodigoBarras });
        }

        [HttpGet]
        public IActionResult ObterPorGenero(string genero)
        {
            var livro = _livroRepository.ObterTodosLivros("");
            var model = new LivroViewModel
            {
                TodosLivros = livro
            };
            ViewBag.Genero = genero;

            return View("ListaProcura", model);
        }

        [HttpGet]
        public IActionResult ObterPorLiteratura(string literatura)
        {
            var livro = _livroRepository.ObterTodosLivros("");
            var model = new LivroViewModel
            {
                TodosLivros = livro
            };
            ViewBag.Literatura = literatura;

            return View("ListaProcura", model);
        }


        [HttpGet]
        public IActionResult ListaProcura(string pesquisa)
        {
            var livro = _livroRepository.ObterTodosLivros(pesquisa);
            var model = new LivroViewModel
            {
                TodosLivros = livro
            };

            return View(model);
        }

    }
}
