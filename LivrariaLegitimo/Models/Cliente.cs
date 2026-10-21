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
        public int Email { get; set; }

        [Display(Name = "Telefone")]
        public int Telefone { get; set; }

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
    }

    public class ClientePF : Cliente
    {
        [Display(Name = "CPF")]
        public long Cpf { get; set; }

        [Display(Name = "Data de Nascimento")]
        public DateOnly DataNasc { get; set; }
    }

    public class ClientePJ : Cliente
    {
        [Display(Name = "CNPJ")]
        public long Cnpj { get; set; }
    }
}