using LivrariaLegitimo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using LivrariaLegitimo.Libraries.Login;
using LivrariaLegitimo.Models;

namespace LivrariaLegitimo.Libraries.Filtro
{
    public class ClienteAutorizacaoAttribute : Attribute, IAuthorizationFilter
    {
        LoginCliente _loginCliente;
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _loginCliente = (LoginCliente)context.HttpContext.RequestServices.GetService(typeof(LoginCliente));
            Cliente cliente = _loginCliente.GetCliente();
            if (cliente == null || cliente.Situacao == "D")
            {
                context.Result = new RedirectToActionResult("LoginCliente", "Home", null);
            }
        }
    }
}