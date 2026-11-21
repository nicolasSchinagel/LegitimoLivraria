
using LivrariaLegitimo.Models;

namespace LivrariaLegitimo.ViewModels
{
    public class CadastroViewModel
    {
        public string TipoCliente { get; set; }

        // Dados compartilhados
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Senha { get; set; }
        public string ConfirmacaoSenha { get; set; }

        public string Cep { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Logradouro { get; set; }
        public int NumeroEndereco { get; set; }
        public string? ComplementoEndereco { get; set; }

        // Dados específicos
        public string? Cpf { get; set; }
        public DateTime? DataNasc { get; set; }
        public string? Cnpj { get; set; }
    }
}
