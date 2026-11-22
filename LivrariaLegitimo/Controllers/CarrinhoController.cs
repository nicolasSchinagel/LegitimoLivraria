using LivrariaLegitimo.CarrinhoCompra;
using LivrariaLegitimo.Libraries.Login;
using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository.Contract;
using LivrariaLegitimo.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
                ObterCliente = _clienteRepository.ObterCliente(_loginCliente.GetCliente().IdCli)
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Carrinho(CompraViewModel model)
        {
            if (model.EditarEndereco.CEP != null)
            {
                if (string.IsNullOrWhiteSpace(model.EditarEndereco.CEP) || string.IsNullOrWhiteSpace(model.EditarEndereco.logradouro) || string.IsNullOrWhiteSpace(model.EditarEndereco.bairro) || string.IsNullOrWhiteSpace(model.EditarEndereco.cidade) || string.IsNullOrWhiteSpace(model.EditarEndereco.estado))
                {
                    ViewBag.Erro = "Preencha todos os campos obrigatórios do endereço.";

                    model.Carrinho = _cookieCarrinhoCompra.Consultar();
                    model.ObterCliente = _clienteRepository.ObterCliente(_loginCliente.GetCliente().IdCli);

                    return View("Carrinho", model);
                }

                _clienteRepository.AtualizarEndereco(
                    model.EditarEndereco.CEP,
                    model.EditarEndereco.logradouro,
                    model.EditarEndereco.bairro,
                    model.EditarEndereco.cidade,
                    model.EditarEndereco.estado,
                    model.EditarEndereco.numeroEndereco,
                    model.EditarEndereco.complementoEndereco
                );

                ViewBag.Sucesso = "Endereço atualizado com sucesso!";

                model.Carrinho = _cookieCarrinhoCompra.Consultar();
                model.ObterCliente = _clienteRepository.ObterCliente(_loginCliente.GetCliente().IdCli);

                return View("Carrinho", model);
            }

            if (model.Compra == null || model.Compra.Itens == null || !model.Compra.Itens.Any())
            {
                ViewBag.Erro = "Não foi possível finalizar a compra. O carrinho está vazio.";

                model.Carrinho = _cookieCarrinhoCompra.Consultar();
                model.ObterCliente = _clienteRepository.ObterCliente(_loginCliente.GetCliente().IdCli);

                return View("Carrinho", model);
            }

            _vendaRepository.Cadastrar(model.Compra);

            TempData["Sucesso"] = "Compra realizada com sucesso!";

            return RedirectToAction("Carrinho");
        }

        [HttpPost]
        public IActionResult Carrinho(Venda venda)
        {
            if (venda == null || venda.Itens == null || !venda.Itens.Any())
            {
                TempData["Erro"] = "Não foi possível finalizar a compra. O carrinho está vazio.";
                return RedirectToAction("Carrinho");
            }

            _vendaRepository.Cadastrar(venda);

            TempData["Sucesso"] = "Compra realizada com sucesso!";

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
