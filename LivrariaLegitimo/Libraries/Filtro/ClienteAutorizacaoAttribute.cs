using LivrariaLegitimo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using LivrariaLegitimo.Libraries.Login;
using LivrariaLegitimo.Models;

namespace LivrariaLegitimo.Libraries.Filtro
{
    public class ClienteAutorizacaoAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var loginCliente = (LoginCliente)context.HttpContext.RequestServices.GetService(typeof(LoginCliente));

            Cliente cliente = loginCliente.GetCliente();

            if (cliente == null)
            {
                context.Result = new RedirectToActionResult("LoginCliente", "Home", null);
            }
        }
    }
}