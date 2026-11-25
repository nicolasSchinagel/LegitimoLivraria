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
        public IActionResult Venda()
        {
            CompraViewModel model = new CompraViewModel
            {
                Carrinho = _cookieCarrinhoCompra.Consultar(),
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

            TempData["Sucesso"] = "Compra realizada com sucesso!";
            _cookieCarrinhoCompra.RemoverTodos();
            return RedirectToAction("Carrinho");
        }
    }
}
