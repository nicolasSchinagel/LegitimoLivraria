using LivrariaLegitimo.Libraries.Login;
using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository.Contract;
using MySql.Data.MySqlClient;
namespace LivrariaLegitimo.Repository
{
    public class VendaRepository : IVendaRepository
    {

        private readonly string _conexaoMySQL;
        IConfiguration _config;
        private readonly LoginCliente _loginCliente;
        private readonly LoginFuncionario _loginFuncionario;
        private readonly ClientePFRepository _clientePFRepository;
        private readonly ClientePJRepository _clientePJRepository;
        public VendaRepository(LoginCliente loginCliente, LoginFuncionario loginFuncionario, ClientePFRepository clientePFRepository,ClientePJRepository clientePJRepository, IConfiguration conf)
        {
            _loginCliente = loginCliente;
            _loginFuncionario = loginFuncionario;
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
            _config = conf;
            _clientePFRepository = clientePFRepository;
            _clientePJRepository = clientePJRepository;
        }

        private int GerarNotaFiscal()
        {
            return Random.Shared.Next(100000000, 1000000000);
        }

        public void Cadastrar(Venda venda)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                var funcionarioIncluso = 0;
                if(_loginFuncionario.GetFuncionario() != null)
                {
                   funcionarioIncluso = _loginFuncionario.GetFuncionario().IdFuncionario;
                }
                venda.NotaFiscal = GerarNotaFiscal();

                conexao.Open();

                if (_loginCliente.GetCliente() != null)
                {
                    if (_clientePFRepository.ObterCPF(_loginCliente.GetCliente().IdCli) != 0) // Quando cliente for Físico
                    {
                        using (var cmd = new MySqlCommand(
                        "call spCadastroVenda(@vNotaFiscal, @vCPFcliente, @vCNPJcliente, @vIdFunc, @vFormaPagamento, @vNumeroCartao, @vNomeCartao, @vValidade, @vCVV, @vNumeroParcelamentos)", conexao))
                        {
                            cmd.Parameters.Add("@vNotaFiscal", MySqlDbType.Int32).Value = venda.NotaFiscal;
                            cmd.Parameters.Add("@vCPFcliente", MySqlDbType.Int64).Value = _clientePFRepository.ObterCPF(_loginCliente.GetCliente().IdCli);
                            cmd.Parameters.Add("@vCNPJcliente", MySqlDbType.Int64).Value = venda.CNPJcliente ?? (object)DBNull.Value;
                            cmd.Parameters.Add("@vIdFunc", MySqlDbType.Int32).Value = DBNull.Value;
                            cmd.Parameters.Add("@vFormaPagamento", MySqlDbType.VarChar).Value = venda.FormaPagamento;
                            cmd.Parameters.Add("@vNumeroCartao", MySqlDbType.Int32).Value = venda.NumeroCartao ?? (object)DBNull.Value;
                            cmd.Parameters.Add("@vNomeCartao", MySqlDbType.VarChar).Value = venda.NomeCartao ?? (object)DBNull.Value;
                            cmd.Parameters.Add("@vValidade", MySqlDbType.Date).Value = venda.ValidadeCartao ?? (object)DBNull.Value; // arrumar conversão da data
                            cmd.Parameters.Add("@vCVV", MySqlDbType.Int32).Value = venda.Cvv ?? (object)DBNull.Value;
                            cmd.Parameters.Add("@vNumeroParcelamentos", MySqlDbType.Int16).Value = venda.NumeroParcelamentos;

                            cmd.ExecuteNonQuery();
                        }
                    }
                    else if (_clientePJRepository.ObterCNPJ(_loginCliente.GetCliente().IdCli) != 0) // Quando Cliente for Jurídico
                    {
                        using (var cmd = new MySqlCommand(
                        "call spCadastroVenda(@vNotaFiscal, @vCPFcliente, @vCNPJcliente, @vIdFunc, @vFormaPagamento, @vNumeroCartao, @vNomeCartao, @vValidade, @vCVV, @vNumeroParcelamentos)", conexao))
                        {
                            cmd.Parameters.Add("@vNotaFiscal", MySqlDbType.Int32).Value = venda.NotaFiscal;
                            cmd.Parameters.Add("@vCPFcliente", MySqlDbType.Int64).Value = venda.CPFcliente ?? (object)DBNull.Value;
                            cmd.Parameters.Add("@vCNPJcliente", MySqlDbType.Int64).Value = _clientePJRepository.ObterCNPJ(_loginCliente.GetCliente().IdCli);
                            cmd.Parameters.Add("@vIdFunc", MySqlDbType.Int32).Value = DBNull.Value;
                            cmd.Parameters.Add("@vFormaPagamento", MySqlDbType.VarChar).Value = venda.FormaPagamento;
                            cmd.Parameters.Add("@vNumeroCartao", MySqlDbType.Int64).Value = venda.NumeroCartao ?? (object)DBNull.Value;
                            cmd.Parameters.Add("@vNomeCartao", MySqlDbType.VarChar).Value = venda.NomeCartao ?? (object)DBNull.Value;
                            cmd.Parameters.Add("@vValidade", MySqlDbType.Date).Value = venda.ValidadeCartao ?? (object)DBNull.Value;
                            cmd.Parameters.Add("@vCVV", MySqlDbType.Int32).Value = venda.Cvv ?? (object)DBNull.Value;
                            cmd.Parameters.Add("@vNumeroParcelamentos", MySqlDbType.Int16).Value = venda.NumeroParcelamentos;

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    using (var cmd = new MySqlCommand(
                    "call spCadastroVenda(@vNotaFiscal, @vCPFcliente, @vCNPJcliente, @vIdFunc, @vFormaPagamento, @vNumeroCartao, @vNomeCartao, @vValidade, @vCVV, @vNumeroParcelamentos)", conexao))
                    {
                        cmd.Parameters.Add("@vNotaFiscal", MySqlDbType.Int32).Value = venda.NotaFiscal;
                        cmd.Parameters.Add("@vCPFcliente", MySqlDbType.Int64).Value = venda.CPFcliente ?? (object)DBNull.Value;
                        cmd.Parameters.Add("@vCNPJcliente", MySqlDbType.Int64).Value = venda.CNPJcliente ?? (object)DBNull.Value;
                        cmd.Parameters.Add("@vIdFunc", MySqlDbType.Int32).Value = funcionarioIncluso;
                        cmd.Parameters.Add("@vFormaPagamento", MySqlDbType.VarChar).Value = venda.FormaPagamento;
                        cmd.Parameters.Add("@vNumeroCartao", MySqlDbType.Int64).Value = venda.NumeroCartao ?? (object)DBNull.Value;
                        cmd.Parameters.Add("@vNomeCartao", MySqlDbType.VarChar).Value = venda.NomeCartao ?? (object)DBNull.Value;
                        cmd.Parameters.Add("@vValidade", MySqlDbType.Date).Value = venda.ValidadeCartao ?? (object)DBNull.Value;
                        cmd.Parameters.Add("@vCVV", MySqlDbType.Int32).Value = venda.Cvv ?? (object)DBNull.Value;
                        cmd.Parameters.Add("@vNumeroParcelamentos", MySqlDbType.Int16).Value = venda.NumeroParcelamentos;

                        cmd.ExecuteNonQuery();
                    }
                }
                // Recuperar número da venda
                int numeroVenda;

                using (var cmdId = new MySqlCommand("SELECT LAST_INSERT_ID();", conexao))
                {
                    numeroVenda = Convert.ToInt32(cmdId.ExecuteScalar());
                }

                // Cadastrar itens da venda
                foreach (var item in venda.Itens)
                {
                    using (var cmdItem = new MySqlCommand(
                        "call spCadastroItemVenda(@vNumeroVenda, @vCodBarras, @vQtdItem)", conexao))
                    {
                        cmdItem.Parameters.Add("@vNumeroVenda", MySqlDbType.Int32).Value = numeroVenda;
                        cmdItem.Parameters.Add("@vCodBarras", MySqlDbType.Decimal).Value = item.CodBarras;
                        cmdItem.Parameters.Add("@vQtdItem", MySqlDbType.Int32).Value = item.QtdItem;

                        cmdItem.ExecuteNonQuery();
                    }
                }
            }
        }

        public IEnumerable<ItemVenda> ObterListaPorNumero(int numeroVenda)
        {
            throw new NotImplementedException();
        }
    }
}
