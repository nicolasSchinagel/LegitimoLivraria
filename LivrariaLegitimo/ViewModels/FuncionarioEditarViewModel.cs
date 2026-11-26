
using System;
using System.ComponentModel.DataAnnotations;

namespace LivrariaLegitimo.ViewModels
{
    public class FuncionarioEditarViewModel
    {
        [Display(Name = "ID Funcionário")]
        public Int32 IdFuncionario { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [StringLength(100, ErrorMessage = "O email deve ter no máximo 100 caracteres")]
        [EmailAddress(ErrorMessage = "Precisa ter formato de email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        // verificar depois para permitir tanto telefone quanto celular
        [Required(ErrorMessage = "Telefone é obrigatório")]
        [RegularExpression(
            @"^\(\d{2}\) \d{5}-\d{4}$",
            ErrorMessage = "Telefone deve estar no formato (11) 12345-6789")]
        [Display(Name = "Telefone")]
        public string Telefone { get; set; }

        [Display(Name = "Cargo")]
        [Required(ErrorMessage = "Cargo é obrigatório")]
        public string Cargo { get; set; }

        [Display(Name = "CEP")]
        [Required(ErrorMessage = "CEP é obrigatório")]
        [RegularExpression(
            @"^\d{5}-?\d{3}$",
            ErrorMessage = "CEP deve estar no formato 12345-678")]
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

        [Display(Name = "Senha")]
        [RegularExpression(
            @"^.{16}$",
            ErrorMessage = "A senha deve ter 16 caracteres")]
        public string? Senha { get; set; }

        [DataType(DataType.Password)]
        [Compare("Senha", ErrorMessage = "As senhas não coincidem")]
        [Display(Name = "Confirmação da senha")]
        public string? ConfirmacaoSenha { get; set; }

        [Required(ErrorMessage = "Número de endereço é obrigatório")]
        [Range(1, 999999, ErrorMessage = "Número de endereço inválido")]
        [Display(Name = "Número do Endereço")]
        public int NumeroEndereco { get; set; }

        [StringLength(100, ErrorMessage = "Complemento deve ter no máximo 100 caracteres")]
        [Display(Name = "Complemento do Endereço")]
        public string? ComplementoEndereco { get; set; }

        [Display(Name = "Fim do Contrato")]
        public DateTime? FimContrato { get; set; }
    }
}

