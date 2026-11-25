using LivrariaLegitimo.CarrinhoCompra;
using LivrariaLegitimo.Libraries.Filtro;
using LivrariaLegitimo.Libraries.Login;
using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository.Contract;
using LivrariaLegitimo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace LivrariaLegitimo.Areas.Worker.Controllers
{
    [Area("Worker")]
    public class VendaController : Controller
    {
        private ILivroRepository _livroRepository;
        private IVendaRepository _vendaRepository;
        private IClienteRepository _clienteRepository;
        private CookieCarrinhoCompra _cookieCarrinhoCompra;
        private LoginFuncionario _loginFuncionario;
        private LoginCliente _loginCliente;
        public VendaController(CookieCarrinhoCompra cookieCarrinhoCompra, ILivroRepository livroRepository, IVendaRepository vendaRepository, IClienteRepository clienteRepository, LoginCliente loginCliente, LoginFuncionario loginFuncionario)
        {
            _cookieCarrinhoCompra = cookieCarrinhoCompra;
            _livroRepository = livroRepository;
            _vendaRepository = vendaRepository;
            _clienteRepository = clienteRepository;
            _loginCliente = loginCliente;
            _loginFuncionario = loginFuncionario;
        }

        [FuncionarioAutorizacao("administrador total", "gerente comercial", "caixa")]
        [HttpGet]
        public IActionResult Venda(string? pesquisa)
        {
            pesquisa ??= "";

            CompraViewModel model = new CompraViewModel
            {
                Carrinho = _cookieCarrinhoCompra.Consultar(),
                TodosLivros = _livroRepository.ObterTodosLivros(pesquisa)
            };

            return View(model);
        }


        [HttpPost]
        [FuncionarioAutorizacao("administrador total", "gerente comercial", "caixa")]
        public IActionResult Venda(CompraViewModel model)
        {
            DateTime? validade = null;

            if (!string.IsNullOrWhiteSpace(model.Compra.ValidadeCartao))
            {
                validade = DateTime.ParseExact(
                    model.Compra.ValidadeCartao,
                    "MM/yy",
                    CultureInfo.InvariantCulture
                );
            }
            model.Compra.IdFuncionario = _loginFuncionario.GetFuncionario().IdFuncionario;
            if (model.Compra == null || model.Compra.Itens == null || !model.Compra.Itens.Any())
            {
                ViewBag.Erro = "Não foi possível finalizar a compra. O carrinho está vazio.";

                model.Carrinho = _cookieCarrinhoCompra.Consultar();

                return View("Comprar", model);
            }

            _vendaRepository.Cadastrar(model.Compra);

            _cookieCarrinhoCompra.RemoverTodos();
            TempData["msg"] = "Cadastro realizado!";
            return RedirectToAction("MenuGerenciar", "Default");
        }


        public IActionResult AdicionarItem(string codBarras)
        {
            _cookieCarrinhoCompra.Cadastrar(codBarras);

            return RedirectToAction(nameof(Venda));
        }


        public IActionResult RemoverItem(string codBarras)
        {
            var Lista = _cookieCarrinhoCompra.Consultar();
            var ItemLocalizado = Lista.SingleOrDefault(a => a.livro.CodigoBarras.ToString() == codBarras);

            if (ItemLocalizado.livro != null)
            {
                _cookieCarrinhoCompra.Remover(ItemLocalizado.livro);
            }

            return RedirectToAction(nameof(Venda));
        }

        public IActionResult LimparCarrinho()
        {
            _cookieCarrinhoCompra.RemoverTodos();

            return RedirectToAction(nameof(Venda));
        }

        public IActionResult AumentarQuantidade(string codBarras)
        {
            _cookieCarrinhoCompra.AumentarQuantidade(codBarras);

            return RedirectToAction(nameof(Venda));
        }


        public IActionResult DiminuirQuantidade(string codBarras)
        {
            _cookieCarrinhoCompra.DiminuirQuantidade(codBarras);

            return RedirectToAction(nameof(Venda));
        }
    }
}
