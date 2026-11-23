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
            _cookieCarrinhoReposicao.RemoverTodos();
            TempData["msg"] = "Cadastro realizado!";
            return RedirectToAction("MenuGerenciar", "Default");
        }


        public IActionResult AdicionarItem(string codBarras)
        {
            _cookieCarrinhoReposicao.Cadastrar(codBarras);

            return RedirectToAction("Reposicao", "Livro");
        }


        public IActionResult RemoverItem(string codBarras)
        {
            var Lista = _cookieCarrinhoReposicao.Consultar();
            var ItemLocalizado = Lista.SingleOrDefault(a => a.livro.CodigoBarras.ToString() == codBarras);

            if (ItemLocalizado.livro != null)
            {
                _cookieCarrinhoReposicao.Remover(ItemLocalizado.livro);
            }

            return RedirectToAction("Reposicao", "Livro");
        }

        public IActionResult LimparReposicao()
        {
            _cookieCarrinhoReposicao.RemoverTodos();

            return RedirectToAction("Reposicao", "Livro");
        }

        public IActionResult AumentarQuantidade(string codBarras)
        {
            _cookieCarrinhoReposicao.AumentarQuantidade(codBarras);

            return RedirectToAction("Reposicao", "Livro");
        }


        public IActionResult DiminuirQuantidade(string codBarras)
        {
            _cookieCarrinhoReposicao.DiminuirQuantidade(codBarras);

            return RedirectToAction("Reposicao", "Livro");
        }

    }
}
