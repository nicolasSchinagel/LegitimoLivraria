using LivrariaLegitimo.Libraries.Login;
using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository;
using LivrariaLegitimo.Repository.Contract;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using System.Diagnostics;

namespace LivrariaLegitimo.Controllers
{
    public class DefaultController : Controller
    {
        private IClienteRepository _clienteRepository;
        private IFuncionarioRepository _funcionarioRepository;
        private LoginFuncionario _loginFuncionario;
        private LoginCliente _loginCliente;
        public DefaultController(IFuncionarioRepository funcionarioRepository, IClienteRepository clienteRepository, LoginFuncionario loginFuncionario, LoginCliente loginCliente)
        {
            _funcionarioRepository = funcionarioRepository;
            _clienteRepository = clienteRepository;
            _loginFuncionario = loginFuncionario;
            _loginCliente = loginCliente;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login()
        {
            return View();
        }





        /*Estou fazendo um login único e mutável, conforme o solicitado, basicamente terá uma única tela de login e será escolhido o tipo de usuário baseado na senha
         Como demonstração, o Cliente só pode ser cadastrado com senha entre 0 a 10 caracteres. O funcionário obrigatoriamente tem 16 caracteres, que vão ser padronizados em
        padrão "X" para letra qualquer e "0" para qualquer número, sendo definido como: "XX00000000XXXXXX", O padrão é uma norma respeitada no cadastrado no funcionário
         */
        
        [HttpPost]
        public IActionResult Login([FromForm] LoginGeral loginGeral)
        {
            if(loginGeral.Senha.Length >= 6 && loginGeral.Senha.Length <= 12)
            {
                Cliente clienteDB = _clienteRepository.Login(loginGeral.Email, loginGeral.Senha); // precisa ajustar o parâmetro

                if (clienteDB.Email != null && clienteDB.Senha != null)
                {
                    _loginCliente.Login(clienteDB);
                    return RedirectToAction("Index");
                }
                else
                {
                    // erro na sessão
                    ViewData["MSG_E"] = "Cliente não localizado, por favor verifique e-mail e senha digitado";
                    return View();
                }
            }
            else if(loginGeral.Senha.Length == 16)
            {
                Models.Funcionario funcionarioDB = _funcionarioRepository.Login(loginGeral.Email, loginGeral.Senha);

                if (funcionarioDB.Email != null && funcionarioDB.Senha != null)
                {
                    _loginFuncionario.Login(funcionarioDB);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["MSG_E"] = "Funcionário não localizado, por favor verifique e-mail e senha digitado";
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
        
    }
}
