using System.ComponentModel.DataAnnotations;

namespace LivrariaLegitimo.Models
{
    public class LoginGeral
    {
        [EmailAddress(ErrorMessage = "O Email não segue um padrão")]
        [Required(ErrorMessage = "O Email é obrigatório")]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [RegularExpression(@"^(.{6,12}|.{16})$", ErrorMessage = "O texto deve ter entre 6 e 12")]
        [Required(ErrorMessage = "A Senha é obrigatória")]
        [Display(Name = "Senha")]
        public string Senha { get; set; }
    }
}
