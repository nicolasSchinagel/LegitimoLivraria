using LivrariaLegitimo.Libraries.Filtro;
using LivrariaLegitimo.Repository.Contract;
using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository;
using Microsoft.AspNetCore.Mvc;
using LivrariaLegitimo.CarrinhoCompra;
using LivrariaLegitimo.ViewModels;

namespace LivrariaLegitimo.Areas.Worker.Controllers
{
    [Area("Worker")]
    public class LivroController : Controller
    {
        private CookieCarrinhoReposicao _cookieCarrinhoReposicao;
        private ILivroRepository _livroRepository;
        public LivroController(ILivroRepository livroRepository, CookieCarrinhoReposicao cookieCarrinhoReposicao)
        {
            _livroRepository = livroRepository;
            _cookieCarrinhoReposicao = cookieCarrinhoReposicao;
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

        [HttpGet]
        [FuncionarioAutorizacao("administrador total", "gerente de compras")]
        public IActionResult Reposicao(string? pesquisa)
        {
            pesquisa ??= ""; // ??= significa que quando a variável for nula, atribui um valor tal, descrito nessa linha
            LivroViewModel model = new LivroViewModel
            {
                TodosLivros = _livroRepository.ObterTodosLivros(pesquisa),
                CookieReposicao = _cookieCarrinhoReposicao.Consultar()
            };
            return View(model);
        }

        [HttpPost]
        [FuncionarioAutorizacao("administrador total", "gerente de compras")]
        public IActionResult Reposicao(List<decimal> codBarras, List<int> quantidade)
        {
            List<(decimal codBarras, int quantidade)> lista = codBarras
                .Select((codigo, i) => (codigo, quantidade[i]))
                .ToList();

            _livroRepository.AdicionarEstoque(lista);

            TempData["msg"] = "Cadastro realizado!";
            return RedirectToAction("MenuGerenciar", "Default");
        }


    }
}
