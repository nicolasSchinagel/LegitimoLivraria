using LivrariaLegitimo.Historicos;
using LivrariaLegitimo.Libraries.Login;
using LivrariaLegitimo.Repository.Contract;
using Microsoft.AspNetCore.Mvc;
using LivrariaLegitimo.Models;
using LivrariaLegitimo.ViewModels;

namespace LivrariaLegitimo.Controllers
{
    public class ClienteController : Controller
    {
        private IClienteRepository _clienteRepository;
        private IClientePFRepository _clientePFRepository;
        private IClientePJRepository _clientePJRepository;
        private IFuncionarioRepository _funcionarioRepository;
        private LoginFuncionario _loginFuncionario;
        private LoginCliente _loginCliente;
        public ClienteController(IFuncionarioRepository funcionarioRepository, IClienteRepository clienteRepository,  LoginFuncionario loginFuncionario, LoginCliente loginCliente, IClientePFRepository clientePFRepository, IClientePJRepository clientePJRepository)
        {
            _funcionarioRepository = funcionarioRepository;
            _clienteRepository = clienteRepository;
            _loginFuncionario = loginFuncionario;
            _loginCliente = loginCliente;
            _clientePFRepository = clientePFRepository;
            _clientePJRepository = clientePJRepository;
        }


        [HttpGet]
        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DadosPF()
        {
            ClientePF clientePF = _clientePFRepository.ObterCliente(_loginCliente.GetCliente().IdCli);
            return View(clientePF);
        }
        [HttpGet]
        public IActionResult DadosPJ()
        {
            ClientePJ clientePJ = _clientePJRepository.ObterCliente(_loginCliente.GetCliente().IdCli);
            return View(clientePJ);
        }

        [HttpPost]
        public IActionResult Cadastro(CadastroViewModel model)
        {
            if (model.TipoCliente == "PF")
            {
                ClientePF clientePF = new ClientePF
                {
                    Nome = model.Nome,
                    Email = model.Email,
                    Telefone = model.Telefone,
                    Senha = model.Senha,
                    ConfirmacaoSenha = model.ConfirmacaoSenha,

                    Cep = model.Cep,
                    Estado = model.Estado,
                    Cidade = model.Cidade,
                    Bairro = model.Bairro,
                    Logradouro = model.Logradouro,
                    NumeroEndereco = model.NumeroEndereco,
                    ComplementoEndereco = model.ComplementoEndereco,

                    Cpf = model.Cpf,
                    DataNasc = model.DataNasc ?? DateTime.MinValue
                };

                if (model.DataNasc == null)
                {
                    ModelState.AddModelError(
                        "DataNasc",
                        "A data de nascimento é obrigatória."
                    );
                }
                TryValidateModel(clientePF);

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                _clientePFRepository.Cadastrar(clientePF);

                TempData["msg"] = "Cadastro realizado!";
                return RedirectToAction("Login", "Default");
            }

            else if (model.TipoCliente == "PJ")
            {
                ClientePJ clientePJ = new ClientePJ
                {
                    Nome = model.Nome,
                    Email = model.Email,
                    Telefone = model.Telefone,
                    Senha = model.Senha,
                    ConfirmacaoSenha = model.ConfirmacaoSenha,

                    Cep = model.Cep,
                    Estado = model.Estado,
                    Cidade = model.Cidade,
                    Bairro = model.Bairro,
                    Logradouro = model.Logradouro,
                    NumeroEndereco = model.NumeroEndereco,
                    ComplementoEndereco = model.ComplementoEndereco,

                    Cnpj = model.Cnpj
                };

                TryValidateModel(clientePJ);

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                _clientePJRepository.Cadastrar(clientePJ);

                TempData["msg"] = "Cadastro realizado!";
                return RedirectToAction("Login", "Default");
            }

            return RedirectToAction("Cadastro");
        }

        [HttpGet]
        public IActionResult Atualizar()
        {
            Cliente cliente = _clienteRepository.ObterCliente(_loginCliente.GetCliente().IdCli);
            return View(cliente);
        }
        [HttpPost]
        public IActionResult Atualizar(Cliente cliente)
        {
            _clienteRepository.AtualizarCliente(cliente);
            TempData["msg"] = "Cadastro realizado!";
            return RedirectToAction("Index", "Default");
        }
    }
}
