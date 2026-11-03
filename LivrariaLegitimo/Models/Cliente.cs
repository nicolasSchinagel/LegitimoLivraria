using System.ComponentModel.DataAnnotations;

namespace LivrariaLegitimo.Models
{
    public class Cliente
    {
        [Display(Name = "ID Cliente")]
        public int IdCli { get; set; }

        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Telefone")]
        public string Telefone { get; set; }

        [Display(Name = "CEP")]
        public string Cep { get; set; }

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
    }

    public class ClientePF : Cliente
    {
        [Display(Name = "CPF")]
        public string Cpf { get; set; }

        [Display(Name = "Data de Nascimento")]
        public DateOnly DataNasc { get; set; }
    }

    public class ClientePJ : Cliente
    {
        [Display(Name = "CNPJ")]
        public string Cnpj { get; set; }
    }
}