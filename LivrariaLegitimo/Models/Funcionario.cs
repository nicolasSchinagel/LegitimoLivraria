using System.ComponentModel.DataAnnotations;

namespace LivrariaLegitimo.Models
{
    public class Funcionario
    {
        [Display(Name = "ID Funcionário")]
        public int IdFuncionario { get; set; }

        [Display(Name = "Email")]
        [RegularExpression(@"^(.{16})$", ErrorMessage = "O texto deve ter 16 caracteres")]
        [Required(ErrorMessage = "Email é obrigatório")]
        public string Email { get; set; }

        [Display(Name = "Telefone")]
        [Required(ErrorMessage ="Telefone é obrigatório")]
        public string Telefone { get; set; }

        [Display(Name = "CPF")]
        [Required(ErrorMessage = "CPF é obrigatório")]
        public string Cpf { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Nome { get; set; }

        [Display(Name = "Cargo")]
        [Required(ErrorMessage = "Cargo é obrigatório")]
        public string Cargo { get; set; }

        [Display(Name = "Data de Admissão")]
        [Required(ErrorMessage = "Data de admissão é obrigatória")]
        public DateTime DataAdmissao { get; set; }

        [Display(Name = "CEP")]
        [Required(ErrorMessage = "CEP é obrigatório")]
        public string Cep { get; set; }

        [Display(Name = "Logradouro")]
        [Required(ErrorMessage = "Logradouro é obrigatório")]
        public string Logradouro { get; set; }

        [Display(Name = "Bairro")]
        [Required(ErrorMessage = "Bairro é obrigatório")]
        public string Bairro { get; set; }

        [Display(Name = "Cidade")]
        [Required(ErrorMessage = "Cidade é obrigatória")]
        public string Cidade { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Estado é obrigatório")]
        public string Estado { get; set; } 

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "Senha é obrigatória")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [DataType(DataType.Password)]
        [Compare("Senha", ErrorMessage = "As senhas não coincidem")]
        [Display(Name = "Confirmação da senha")]
        public string ConfirmacaoSenha { get; set; }

        [Display(Name = "Número do Endereço")]
        [Required(ErrorMessage = "Número do endereço é obrigatório")]
        public int NumeroEndereco { get; set; }

        [Display(Name = "Complemento do Endereço")]
        public string? ComplementoEndereco { get; set; }


        [Display(Name = "Fim do Contrato")]
        public DateTime? FimContrato { get; set; }
    }
}
