using LivrariaLegitimo.Models;

namespace LivrariaLegitimo.ViewModels
{
    public class EnderecoViewModel
    {
        public string CEP { get; set; }
        public string logradouro { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string estado { get; set; }
        public int numeroEndereco { get; set; }
        public string complementoEndereco { get; set; }

        public Cliente ObterCliente { get; set; }
    }
}
