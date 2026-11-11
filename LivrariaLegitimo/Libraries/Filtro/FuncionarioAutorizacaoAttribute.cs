using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using LivrariaLegitimo.Models;
using LivrariaLegitimo.Libraries.Login;
namespace LivrariaLegitimo.Libraries.Filtro
{


    public class FuncionarioAutorizacaoAttribute : Attribute, IAuthorizationFilter
    {
        private string[] _tiposFuncionario;

        public FuncionarioAutorizacaoAttribute(params string[] tiposFuncionario)
        {
            _tiposFuncionario = tiposFuncionario;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var loginFuncionario = context.HttpContext.RequestServices
                .GetService(typeof(LoginFuncionario)) as LoginFuncionario;

            Funcionario funcionario = loginFuncionario.GetFuncionario();

            if (funcionario == null)
            {
                context.Result = new RedirectToActionResult("Login", "Default", null);
            }
            else
            {
                if (_tiposFuncionario.Length > 0 &&
                    !_tiposFuncionario.Contains(funcionario.Cargo))
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }


}
