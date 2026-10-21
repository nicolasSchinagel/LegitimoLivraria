using System.ComponentModel.DataAnnotations;

namespace LivrariaLegitimo.Models
{
    public class Funcionario
    {
        [Display(Name = "ID Funcionário")]
        public int IdFuncionario { get; set; }

        [Display(Name = "Email")]
        public int Email { get; set; }

        [Display(Name = "Telefone")]
        public int Telefone { get; set; }

        [Display(Name = "CPF")]
        public long Cpf { get; set; }

        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Display(Name = "Cargo")]
        public string Cargo { get; set; }

        [Display(Name = "Data de Admissão")]
        public DateTime DataAdmissao { get; set; }

        [Display(Name = "CEP")]
        public decimal Cep { get; set; }

        [Display(Name = "Logradouro")]
        public string Logradouro { get; set; }

        [Display(Name = "Bairro")]
        public string Bairro { get; set; }

        [Display(Name = "Cidade")]
        public string Cidade { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; } 

        [Display(Name = "Senha")]
        public string Senha { get; set; }

        [Display(Name = "Número do Endereço")]
        public int NumeroEndereco { get; set; }

        [Display(Name = "Complemento do Endereço")]
        public string? ComplementoEndereco { get; set; }

        [Display(Name = "Fim do Contrato")]
        public DateOnly FimContrato { get; set; }
    }
}
