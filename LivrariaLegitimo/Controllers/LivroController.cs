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
        private CookieHistoricoEbook _cookieHistoricoEbook;
        public LivroController(CookieHistoricoLivro cookieHistoricoLivro, CookieHistoricoEbook cookieHistoricoEbook, IClienteRepository clienteRepository, IFuncionarioRepository funcionarioRepository, LoginFuncionario loginFuncionario, LoginCliente loginCliente, ILivroRepository livroRepository, IAvaliacaoRepository avaliacaoRepository)
        {
            _funcionarioRepository = funcionarioRepository;
            _clienteRepository = clienteRepository;
            _loginFuncionario = loginFuncionario;
            _loginCliente = loginCliente;
            _livroRepository = livroRepository;
            _avaliacaoRepository = avaliacaoRepository;
            _cookieHistoricoLivro = cookieHistoricoLivro;
            _cookieHistoricoEbook = cookieHistoricoEbook;
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

        [HttpGet]
        public IActionResult Ebook(string codBarras)
        {
            var livro = _livroRepository.ObterLivro(codBarras);
            var quantidades = _avaliacaoRepository.ObterQuantidadesAvaliacaoLivro(codBarras);
            var avaliacoes = _avaliacaoRepository.ObterTodasAvaliacoes(codBarras);
            var existeAvaliacao = _avaliacaoRepository.ExisteAvaliacaoConta(codBarras);
            var aleatorios = _livroRepository.AleatorizarLivros(codBarras);

            var historico = _cookieHistoricoEbook.Consultar()
                .FirstOrDefault(x => x.livro != null &&
                                     x.livro.CodigoBarras.ToString() == codBarras);

            int paginaAtual = historico.livro != null ? historico.paginaAtual : 1;
            int paginaTotal = historico.livro != null ? historico.paginaTotal : 1;

            var model = new LivroViewModel
            {
                ObterLivro = livro,
                ObterQuantidadesAvaliacaoLivro = quantidades,
                ObterTodasAvaliacoes = avaliacoes,
                ExisteAvaliacaoConta = existeAvaliacao,
                Aleatorios = aleatorios,

                HistoricoE = new List<(Livro livro, string arquivoImagem, string arquivoEbook, int paginaAtual, int paginaTotal)>
        {
            (livro.livro, livro.arquivoImagem, livro.arquivoEbook, paginaAtual, paginaTotal)
        }
            };

            // Garante que o livro exista no histórico.
            // Os valores reais serão atualizados assim que o PDF carregar.
            if (historico.livro == null)
            {
                _cookieHistoricoEbook.Cadastrar(codBarras, 1, 1);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult AtualizarProgressoEbook([FromBody] ProgressoEbookViewModel dados)
        {
            if (dados == null ||
                string.IsNullOrWhiteSpace(dados.CodBarras) ||
                dados.PaginaAtual < 1 ||
                dados.PaginaTotal < 1 ||
                dados.PaginaAtual > dados.PaginaTotal)
            {
                return BadRequest();
            }

            bool atualizado = _cookieHistoricoEbook.AtualizarProgresso(
                dados.CodBarras,
                dados.PaginaAtual,
                dados.PaginaTotal
            );

            if (!atualizado)
                return NotFound();

            return Ok();
        }
    }
}
