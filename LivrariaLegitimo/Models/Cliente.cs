using System.ComponentModel.DataAnnotations;

namespace LivrariaLegitimo.Models
{
    public class Cliente
    {
        [Display(Name = "ID Cliente")]
        public int IdCli { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [StringLength(100, ErrorMessage = "O email deve ter no máximo 100 caracteres")]
        [EmailAddress(ErrorMessage = "Precisa ter formato de email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefone é obrigatório")]
        [StringLength(11, MinimumLength = 10, ErrorMessage = "Telefone deve conter 10 ou 11 dígitos")]
        [Display(Name = "Telefone")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "CEP é obrigatório")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "CEP deve conter 8 dígitos")]
        [Display(Name = "CEP")]
        public string Cep { get; set; }

        [Required(ErrorMessage = "Logradouro é obrigatório")]
        [StringLength(100, ErrorMessage = "Logradouro deve ter no máximo 100 caracteres")]
        [Display(Name = "Logradouro")]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = "Bairro é obrigatório")]
        [StringLength(100, ErrorMessage = "Bairro deve ter no máximo 100 caracteres")]
        [Display(Name = "Bairro")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "Cidade é obrigatória")]
        [StringLength(100, ErrorMessage = "Cidade deve ter no máximo 100 caracteres")]
        [Display(Name = "Cidade")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Estado é obrigatório")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Estado deve conter 2 caracteres")]
        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        [RegularExpression(@"^(.{6,12}|.{16})$", ErrorMessage = "A senha deve ter entre 6 e 12")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Confirmação da senha é obrigatória")]
        [RegularExpression(@"^(.{6,12}|.{16})$", ErrorMessage = "A senha deve ter entre 6 e 12")]
        [DataType(DataType.Password)]
        [Compare("Senha", ErrorMessage = "As senhas não coincidem")]
        [Display(Name = "Confirmação da senha")]
        public string ConfirmacaoSenha { get; set; }

        [Required(ErrorMessage = "Número de endereço é obrigatório")]
        [Range(1, 999999, ErrorMessage = "Número de endereço inválido")]
        [Display(Name = "Número do Endereço")]
        public int NumeroEndereco { get; set; }

        [StringLength(100, ErrorMessage = "Complemento deve ter no máximo 100 caracteres")]
        [Display(Name = "Complemento do Endereço")]
        public string? ComplementoEndereco { get; set; }
    }

    public class ClientePF : Cliente
    {
        [Display(Name = "CPF")]
        [Required(ErrorMessage = "CPF é obrigatório")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve conter 11 dígitos")]
        public string Cpf { get; set; }

        [Display(Name = "Data de Nascimento")]
        [Required(ErrorMessage = "Data de nascimento é obrigatória")]
        [IdadeMinima(18, ErrorMessage = "É necessário ter pelo menos 18 anos para se cadastrar")]
        public DateTime DataNasc { get; set; }
    }

    public class ClientePJ : Cliente
    {
        [Display(Name = "CNPJ")]
        [Required(ErrorMessage = "CNPJ é obrigatório")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "CNPJ deve conter 14 dígitos")]
        public string Cnpj { get; set; }
    }


    // Validação personalizada para verificar idades indisponíveis
    public class IdadeMinimaAttribute : ValidationAttribute
    {
        private readonly int _idadeMinima;

        public IdadeMinimaAttribute(int idadeMinima)
        {
            _idadeMinima = idadeMinima;
        }

        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext)
        {
            if (value is not DateTime dataNascimento)
                return ValidationResult.Success;

            if (dataNascimento.Date > DateTime.Today)
            {
                return new ValidationResult("A data de nascimento não pode ser futura");
            }

            DateTime dataLimite = DateTime.Today.AddYears(-_idadeMinima);

            if (dataNascimento.Date > dataLimite)
            {
                return new ValidationResult(
                    ErrorMessage ?? $"É necessário ter pelo menos {_idadeMinima} anos");
            }

            return ValidationResult.Success;
        }
    }
}