using LivrariaLegitimo.CarrinhoCompra;
using LivrariaLegitimo.Libraries.Login;
using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository.Contract;
using LivrariaLegitimo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace LivrariaLegitimo.Controllers
{
    public class CarrinhoController : Controller
    {
        private ILivroRepository _livroRepository;
        private IVendaRepository _vendaRepository;
        private IClienteRepository _clienteRepository;
        private CookieCarrinhoCompra _cookieCarrinhoCompra;
        private LoginFuncionario _loginFuncionario;
        private LoginCliente _loginCliente;
        public CarrinhoController(CookieCarrinhoCompra cookieCarrinhoCompra, ILivroRepository livroRepository, IVendaRepository vendaRepository, IClienteRepository clienteRepository, LoginCliente loginCliente, LoginFuncionario loginFuncionario) { 
            _cookieCarrinhoCompra = cookieCarrinhoCompra;
            _livroRepository = livroRepository;
            _vendaRepository = vendaRepository;
            _clienteRepository = clienteRepository;
            _loginCliente = loginCliente;
            _loginFuncionario = loginFuncionario;
        }


        [HttpGet]
        public IActionResult Carrinho()
        {
            CompraViewModel model = new CompraViewModel
            {
                Carrinho = _cookieCarrinhoCompra.Consultar(),
            };

            return View(model);
        }


        [HttpGet]
        public IActionResult EditarEndereco()
        {
            EnderecoViewModel model = new EnderecoViewModel
            {
                ObterCliente = _clienteRepository.ObterCliente(_loginCliente.GetCliente().IdCli)
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditarEndereco(EnderecoViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.CEP) ||
                string.IsNullOrWhiteSpace(model.logradouro) ||
                string.IsNullOrWhiteSpace(model.bairro) ||
                string.IsNullOrWhiteSpace(model.cidade) ||
                string.IsNullOrWhiteSpace(model.estado))
            {
                ViewBag.Erro = "Preencha todos os campos obrigatórios do endereço.";
                return RedirectToAction("EditarEndereco");
            }

            _clienteRepository.AtualizarEndereco(
                model.CEP,
                model.logradouro,
                model.bairro,
                model.cidade,
                model.estado,
                model.numeroEndereco,
                model.complementoEndereco
            );

            TempData["Sucesso"] = "Endereço atualizado com sucesso!";

            return RedirectToAction("Comprar");
        }


        [HttpGet]
        public IActionResult Comprar()
        {
            CompraViewModel model = new CompraViewModel
            {
                Carrinho = _cookieCarrinhoCompra.Consultar(),
                ObterCliente = _clienteRepository.ObterCliente(_loginCliente.GetCliente().IdCli)
            };

            return View(model);
        }


        [HttpPost]
        public IActionResult Comprar(CompraViewModel model)
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
            if (model.Compra == null || model.Compra.Itens == null || !model.Compra.Itens.Any())
            {
                ViewBag.Erro = "Não foi possível finalizar a compra. O carrinho está vazio.";

                model.Carrinho = _cookieCarrinhoCompra.Consultar();
                model.ObterCliente = _clienteRepository.ObterCliente(_loginCliente.GetCliente().IdCli);

                return View("Comprar", model);
            }

            _vendaRepository.Cadastrar(model.Compra);

            TempData["Sucesso"] = "Compra realizada com sucesso!";
            _cookieCarrinhoCompra.RemoverTodos();
            return RedirectToAction("Carrinho");
        }



        public IActionResult AdicionarItem(string codBarras)
        {
            _cookieCarrinhoCompra.Cadastrar(codBarras);

            return RedirectToAction(nameof(Carrinho));
        }


        public IActionResult RemoverItem(string codBarras)
        {
            var Lista = _cookieCarrinhoCompra.Consultar();
            var ItemLocalizado = Lista.SingleOrDefault(a => a.livro.CodigoBarras.ToString() == codBarras);

            if (ItemLocalizado.livro != null)
            {
                _cookieCarrinhoCompra.Remover(ItemLocalizado.livro);
            }

            return RedirectToAction(nameof(Carrinho));
        }

        public IActionResult LimparCarrinho()
        {
            _cookieCarrinhoCompra.RemoverTodos();

            return RedirectToAction(nameof(Carrinho));
        }

        public IActionResult AumentarQuantidade(string codBarras)
        {
            _cookieCarrinhoCompra.AumentarQuantidade(codBarras);

            return RedirectToAction(nameof(Carrinho));
        }

        
        public IActionResult DiminuirQuantidade(string codBarras)
        {
            _cookieCarrinhoCompra.DiminuirQuantidade(codBarras);

            return RedirectToAction(nameof(Carrinho));
        }
    }
}
