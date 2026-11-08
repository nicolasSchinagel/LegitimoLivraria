using System.ComponentModel.DataAnnotations;

namespace LivrariaLegitimo.Models
{
    public class LoginGeral
    {
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Senha")]
        public string Senha { get; set; }
    }
}
