using LivrariaLegitimo.GerenciaArquivos;
using LivrariaLegitimo.Libraries.Login;
using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository.Contract;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Text;
using X.PagedList;
using X.PagedList.Extensions;
using System.Text;

namespace LivrariaLegitimo.Repository
{
    public class LivroRepository : ILivroRepository
    {
        private readonly string _conexaoMySQL;
        IConfiguration _config;
        private readonly LoginCliente _loginCliente;
        private readonly LoginFuncionario _loginFuncionario;
        public LivroRepository(LoginCliente loginCliente, LoginFuncionario loginFuncionario, IConfiguration conf)
        {
            _loginCliente = loginCliente;
            _loginFuncionario = loginFuncionario;
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
            _config = conf;
        }


        public (Livro livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas) ObterLivro(string codBarras)
        {

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                string caminhoImagem = "";
                string caminhoEbook = "";
                decimal mediaLivro = 0;
                int numeroVendas = 0;
                MySqlCommand cmd = new MySqlCommand("select li.*, va.MediaAvaliacao, vv.NumeroVendas from ViewLivro li left join ViewLivroAvaliacoes va on li.codBarras = va.codBarras left join ViewLivroVendas vv on li.codBarras = vv.codBarras where(li.CodBarras = @CodBarras)", conexao);
                cmd.Parameters.AddWithValue("@CodBarras", Convert.ToDecimal(codBarras));

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Livro livro = new Livro();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    livro.CodigoBarras = (decimal)(dr["codBarras"]);
                    livro.ISBN = Convert.ToUInt64(dr["ISBN"]).ToString(@"000-0-00-000000-0");
                    livro.Nome = (string)(dr["Nome"]);
                    livro.Preco = Convert.ToDecimal(dr["Preco"]).ToString("N2", CultureInfo.GetCultureInfo("pt-BR"));
                    livro.Sinopse = (string)(dr["Sinopse"]);
                    livro.Autor = (string)(dr["Autor"]);
                    livro.Editora = (string)(dr["Editora"]);
                    livro.IdadeRecomendada = (string)(dr["IdadeMinRecomendada"]);
                    livro.DataPubli = DateOnly.FromDateTime(Convert.ToDateTime(dr["DataPublicacao"]));
                    livro.TipoCapa = (string)(dr["Tipo"]);
                    livro.Idioma = (string)(dr["Idioma"]);
                    livro.Literatura = (string)(dr["Literatura"]);
                    livro.Tamanho = (string)(dr["Tamanho"]);
                    caminhoImagem = (string)(dr["Imagem"]);
                    livro.Quantidade = (int)(dr["Quantidade"]);
                    livro.PromocaoPorcentagem = (int)(dr["PromocaoPorcentagem"]);
                    livro.Capacidade = (int)(dr["capacidadeEstoque"]);
                    livro.Generos = dr["Generos"] == DBNull.Value ? new List<string>() : dr["Generos"].ToString().Split(", ").ToList();
                    mediaLivro = Convert.ToDecimal(dr["MediaAvaliacao"]);
                    numeroVendas = Convert.ToInt32(dr["NumeroVendas"]);
                }
                return (livro, caminhoImagem, caminhoEbook, mediaLivro, numeroVendas);

            }
        }

        public IEnumerable<(Livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> ObterTodosLivros(string pesquisa)
        {
            List<(Livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> listaLivro = new List<(Livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                string consulta = "select li.*, va.MediaAvaliacao, vv.NumeroVendas from ViewLivro li inner join ViewLivroAvaliacoes va on li.codBarras = va.codBarras inner join ViewLivroVendas vv on li.codBarras = vv.codBarras where(li.Quantidade > 0) order by li.Nome";

                if (_loginFuncionario.GetFuncionario() != null)
                {
                    consulta = "select li.*, va.MediaAvaliacao, vv.NumeroVendas from ViewLivro li inner join ViewLivroAvaliacoes va on li.codBarras = va.codBarras inner join ViewLivroVendas vv on li.codBarras = vv.codBarras order by li.Nome";
                }

                MySqlCommand cmd = new MySqlCommand(consulta, conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    listaLivro.Add((
                        new Livro
                        {
                            CodigoBarras = (decimal)(dr["codBarras"]),
                            ISBN = Convert.ToUInt64(dr["ISBN"]).ToString(@"000-0-00-000000-0"),
                            Nome = (string)(dr["Nome"]),
                            Preco = Convert.ToDecimal(dr["Preco"]).ToString("N2", CultureInfo.GetCultureInfo("pt-BR")),
                            Sinopse = (string)(dr["Sinopse"]),
                            Autor = (string)(dr["Autor"]),
                            Editora = (string)(dr["Editora"]),
                            IdadeRecomendada = (string)(dr["IdadeMinRecomendada"]),
                            DataPubli = DateOnly.FromDateTime(Convert.ToDateTime(dr["DataPublicacao"])),
                            TipoCapa = (string)(dr["Tipo"]),
                            Idioma = (string)(dr["Idioma"]),
                            Literatura = (string)(dr["Literatura"]),
                            Tamanho = (string)(dr["Tamanho"]),
                            Quantidade = (int)(dr["Quantidade"]),
                            Capacidade = (int)(dr["capacidadeEstoque"]),
                            Generos = dr["Generos"] == DBNull.Value ? new List<string>() : dr["Generos"].ToString().Split(", ").ToList(),
                            PromocaoPorcentagem = (int)(dr["PromocaoPorcentagem"])
                        },
                        dr["Imagem"].ToString(),
                        dr["ArquivoEbook"].ToString(),
                        Convert.ToDecimal(dr["MediaAvaliacao"]),
                        Convert.ToInt32(dr["NumeroVendas"])
                    ));
                }

                if (string.IsNullOrWhiteSpace(pesquisa))
                {
                    return listaLivro.OrderBy(item => item.Item1.Nome);
                }

                listaLivro = listaLivro
                    .Where(item =>
                        MenorDistancia(pesquisa, item.Item1.Nome) <= 2 ||
                        MenorDistancia(pesquisa, item.Item1.Autor) <= 2 ||
                        MenorDistancia(pesquisa, item.Item1.Editora) <= 2)
                    .OrderBy(item => Math.Min(
                        MenorDistancia(pesquisa, item.Item1.Nome),
                        Math.Min(
                            MenorDistancia(pesquisa, item.Item1.Autor),
                            MenorDistancia(pesquisa, item.Item1.Editora))))
                    .ThenBy(item => item.Item1.Nome)
                    .ToList();

                return listaLivro;
            }
        }

        // Método auxiliar da pesquisa, que permite variancia de algumas letras
        private int MenorDistancia(string pesquisa, string atributo)
        {
            string pesquisaFormatada = NormalizarTexto(pesquisa);
            string atributoFormatado = NormalizarTexto(atributo);

            if (atributoFormatado.Contains(pesquisaFormatada))
            {
                return 0;
            }

            int menorDistancia = DistanciaLevenshtein(pesquisaFormatada, atributoFormatado);

            foreach (string palavra in atributoFormatado.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                int distancia = DistanciaLevenshtein(pesquisaFormatada, palavra);

                if (distancia < menorDistancia)
                {
                    menorDistancia = distancia;
                }
            }

            return menorDistancia;
        }
        // Normaliza o texto para evitar diferenças entre LoweCase e UpperCase, maiúsculo e minúsculo
        private string NormalizarTexto(string texto)
        {
            return string.Concat(
                texto.ToLower().Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            ).Normalize(NormalizationForm.FormC);
        }
        // calcula diferença entre palavras
        private int DistanciaLevenshtein(string palavra1, string palavra2)
        {
            int[,] matriz = new int[palavra1.Length + 1, palavra2.Length + 1];

            for (int i = 0; i <= palavra1.Length; i++)
            {
                matriz[i, 0] = i;
            }

            for (int j = 0; j <= palavra2.Length; j++)
            {
                matriz[0, j] = j;
            }

            for (int i = 1; i <= palavra1.Length; i++)
            {
                for (int j = 1; j <= palavra2.Length; j++)
                {
                    int custo = palavra1[i - 1] == palavra2[j - 1] ? 0 : 1;

                    matriz[i, j] = Math.Min(
                        Math.Min(
                            matriz[i - 1, j] + 1,
                            matriz[i, j - 1] + 1),
                        matriz[i - 1, j - 1] + custo);
                }
            }

            return matriz[palavra1.Length, palavra2.Length];
        }


        // Fazendo cadastro do Livro bem detalhado :)
        public void Cadastrar(Livro livro) {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                string caminhoEbook;
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("call spAdicionarLivro(@CodBarras, @ISBN , @Nome, @Preco , @Sinopse , @Autor, @Editora, @IdadeMinRecomendada, @DataPublicacao, @Tipo, @Idioma, @Literatura, @Tamanho, @ImagemPri, @EbookFile, @CapacidadeEstoque);", conexao);
                var caminhoImagem = GerenciadorCapa.CadastrarCapaLivro(livro.Imagem);
                if(livro.TipoCapa == "Ebook")
                {
                    caminhoEbook = GerenciadorEbook.CadastrarArquivoEbook(livro.ArquivoEbook);
                }
                else
                {
                    caminhoEbook = "";
                }

                cmd.Parameters.Add("@CodBarras", MySqlDbType.Decimal).Value = livro.CodigoBarras;
                cmd.Parameters.Add("@ISBN", MySqlDbType.Decimal).Value = Convert.ToDecimal(livro.ISBN.Replace("-", ""));
                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = livro.Nome;
                cmd.Parameters.Add("@Preco", MySqlDbType.Decimal).Value = decimal.Parse(livro.Preco.Replace(".", "").Replace(",", "."), CultureInfo.InvariantCulture);
                cmd.Parameters.Add("@Sinopse", MySqlDbType.Text).Value = livro.Sinopse;
                cmd.Parameters.Add("@Autor", MySqlDbType.VarChar).Value = livro.Autor;
                cmd.Parameters.Add("@Editora", MySqlDbType.VarChar).Value = livro.Editora;
                cmd.Parameters.Add("@IdadeMinRecomendada", MySqlDbType.VarChar).Value = livro.IdadeRecomendada;
                cmd.Parameters.Add("@DataPublicacao", MySqlDbType.Date).Value = livro.DataPubli;
                cmd.Parameters.Add("@Tipo", MySqlDbType.VarChar).Value = livro.TipoCapa;
                cmd.Parameters.Add("@Idioma", MySqlDbType.VarChar).Value = livro.Idioma;
                cmd.Parameters.Add("@Literatura", MySqlDbType.VarChar).Value = livro.Literatura;
                cmd.Parameters.Add("@Tamanho", MySqlDbType.VarChar).Value = livro.Tamanho;
                cmd.Parameters.Add("@ImagemPri", MySqlDbType.VarChar).Value = caminhoImagem;
                cmd.Parameters.Add("@EbookFile", MySqlDbType.VarChar).Value = caminhoEbook;
                cmd.Parameters.Add("@CapacidadeEstoque", MySqlDbType.Int32).Value = livro.Capacidade;

                cmd.ExecuteNonQuery();
                // Como se trata de uma lista de gêneros, adicionamos uma por uma aqui, com outra procedure
                foreach (string genero in livro.Generos)
                {
                    MySqlCommand cmdGenero = new MySqlCommand("call spAdicionarLivroGenero(@Genero, @CodBarras);", conexao);

                    cmdGenero.Parameters.Add("@Genero", MySqlDbType.VarChar).Value = genero;
                    cmdGenero.Parameters.Add("@CodBarras", MySqlDbType.Decimal).Value = livro.CodigoBarras;

                    cmdGenero.ExecuteNonQuery();
                }

                conexao.Close();
            }
        }
        public void ModificarPreco(int promocao, decimal preco) { throw new NotImplementedException(); }
        public void AtualizarHistoricoG(Livro livro) { throw new NotImplementedException(); }
        public void AtualizarHistoricoE(Livro livro) { throw new NotImplementedException(); }


        public IEnumerable<(Livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> ObterMelhoresAvaliados()
        {
            List<(Livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> listaLivro = new List<(Livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand consulta = new MySqlCommand(
                    "select li.*, vv.NumeroVendas, va.MediaAvaliacao from ViewLivro li inner join ViewLivroVendas vv on li.codBarras = vv.codBarras inner join ViewLivroAvaliacoes va on li.codBarras = va.codBarras where(Quantidade > 0) order by va.MediaAvaliacao desc, vv.NumeroVendas desc limit 5",
                    conexao
                );

                if (_loginFuncionario.GetFuncionario() != null)
                {
                    consulta = new MySqlCommand(
                        "select li.*, vv.NumeroVendas, va.MediaAvaliacao from ViewLivro li inner join ViewLivroVendas vv on li.codBarras = vv.codBarras inner join ViewLivroAvaliacoes va on li.codBarras = va.codBarras order by va.MediaAvaliacao desc, vv.NumeroVendas desc limit 5",
                        conexao
                    );
                }

                MySqlDataAdapter da = new MySqlDataAdapter(consulta);
                DataTable dt = new DataTable();

                da.Fill(dt);

                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    listaLivro.Add((
                        new Livro
                        {
                            CodigoBarras = (decimal)(dr["codBarras"]),
                            ISBN = Convert.ToUInt64(dr["ISBN"]).ToString(@"000-0-00-000000-0"),
                            Nome = (string)(dr["Nome"]),
                            Preco = Convert.ToDecimal(dr["Preco"]).ToString("N2", CultureInfo.GetCultureInfo("pt-BR")),
                            Sinopse = (string)(dr["Sinopse"]),
                            Autor = (string)(dr["Autor"]),
                            Editora = (string)(dr["Editora"]),
                            IdadeRecomendada = (string)(dr["IdadeMinRecomendada"]),
                            DataPubli = DateOnly.FromDateTime(Convert.ToDateTime(dr["DataPublicacao"])),
                            TipoCapa = (string)(dr["Tipo"]),
                            Idioma = (string)(dr["Idioma"]),
                            Literatura = (string)(dr["Literatura"]),
                            Tamanho = (string)(dr["Tamanho"]),
                            Quantidade = (int)(dr["Quantidade"]),
                            Capacidade = (int)(dr["capacidadeEstoque"]),
                            Generos = dr["Generos"] == DBNull.Value ? new List<string>() : dr["Generos"].ToString().Split(", ").ToList(),
                            PromocaoPorcentagem = (int)(dr["PromocaoPorcentagem"])
                        },
                        dr["Imagem"].ToString(),
                        dr["ArquivoEbook"].ToString(),
                        Convert.ToDecimal(dr["MediaAvaliacao"]),
                        Convert.ToInt32(dr["NumeroVendas"])
                    ));
                }

                return listaLivro;
            }
        }

        public IEnumerable<(Livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> AleatorizarLivros()
        {
            List<(Livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> listaLivro = new List<(Livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand consulta = new MySqlCommand(
                    "select li.*, vv.NumeroVendas, va.MediaAvaliacao from ViewLivro li inner join ViewLivroVendas vv on li.codBarras = vv.codBarras inner join ViewLivroAvaliacoes va on li.codBarras = va.codBarras where(Quantidade > 0) order by RAND() limit 9",
                    conexao
                );

                if (_loginFuncionario.GetFuncionario() != null)
                {
                    consulta = new MySqlCommand(
                        "select li.*, vv.NumeroVendas, va.MediaAvaliacao from ViewLivro li inner join ViewLivroVendas vv on li.codBarras = vv.codBarras inner join ViewLivroAvaliacoes va on li.codBarras = va.codBarras order by RAND() limit 9",
                        conexao
                    );
                }

                MySqlDataAdapter da = new MySqlDataAdapter(consulta);
                DataTable dt = new DataTable();

                da.Fill(dt);

                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    listaLivro.Add((
                        new Livro
                        {
                            CodigoBarras = (decimal)(dr["codBarras"]),
                            ISBN = Convert.ToUInt64(dr["ISBN"]).ToString(@"000-0-00-000000-0"),
                            Nome = (string)(dr["Nome"]),
                            Preco = Convert.ToDecimal(dr["Preco"]).ToString("N2", CultureInfo.GetCultureInfo("pt-BR")),
                            Sinopse = (string)(dr["Sinopse"]),
                            Autor = (string)(dr["Autor"]),
                            Editora = (string)(dr["Editora"]),
                            IdadeRecomendada = (string)(dr["IdadeMinRecomendada"]),
                            DataPubli = DateOnly.FromDateTime(Convert.ToDateTime(dr["DataPublicacao"])),
                            TipoCapa = (string)(dr["Tipo"]),
                            Idioma = (string)(dr["Idioma"]),
                            Literatura = (string)(dr["Literatura"]),
                            Tamanho = (string)(dr["Tamanho"]),
                            Quantidade = (int)(dr["Quantidade"]),
                            Capacidade = (int)(dr["capacidadeEstoque"]),
                            Generos = dr["Generos"] == DBNull.Value ? new List<string>() : dr["Generos"].ToString().Split(", ").ToList(),
                            PromocaoPorcentagem = (int)(dr["PromocaoPorcentagem"])
                        },
                        dr["Imagem"].ToString(),
                        dr["ArquivoEbook"].ToString(),
                        Convert.ToDecimal(dr["MediaAvaliacao"]),
                        Convert.ToInt32(dr["NumeroVendas"])
                    ));
                }

                return listaLivro;
            }
        }

        public IEnumerable<(Livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> ObterMaioresOfertas()
        {
            List<(Livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> listaLivro = new List<(Livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand consulta = new MySqlCommand(
                    "select li.*, vv.NumeroVendas, va.MediaAvaliacao from ViewLivro li inner join ViewLivroVendas vv on li.codBarras = vv.codBarras inner join ViewLivroAvaliacoes va on li.codBarras = va.codBarras where(Quantidade > 0) order by PromocaoPorcentagem DESC limit 2",
                    conexao
                );

                if (_loginFuncionario.GetFuncionario() != null)
                {
                    consulta = new MySqlCommand(
                        "select li.*, vv.NumeroVendas, va.MediaAvaliacao from ViewLivro li inner join ViewLivroVendas vv on li.codBarras = vv.codBarras inner join ViewLivroAvaliacoes va on li.codBarras = va.codBarras order by PromocaoPorcentagem DESC limit 2",
                        conexao
                    );
                }

                MySqlDataAdapter da = new MySqlDataAdapter(consulta);
                DataTable dt = new DataTable();

                da.Fill(dt);

                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    listaLivro.Add((
                        new Livro
                        {
                            CodigoBarras = (decimal)(dr["codBarras"]),
                            ISBN = Convert.ToUInt64(dr["ISBN"]).ToString(@"000-0-00-000000-0"),
                            Nome = (string)(dr["Nome"]),
                            Preco = Convert.ToDecimal(dr["Preco"]).ToString("N2", CultureInfo.GetCultureInfo("pt-BR")),
                            Sinopse = (string)(dr["Sinopse"]),
                            Autor = (string)(dr["Autor"]),
                            Editora = (string)(dr["Editora"]),
                            IdadeRecomendada = (string)(dr["IdadeMinRecomendada"]),
                            DataPubli = DateOnly.FromDateTime(Convert.ToDateTime(dr["DataPublicacao"])),
                            TipoCapa = (string)(dr["Tipo"]),
                            Idioma = (string)(dr["Idioma"]),
                            Literatura = (string)(dr["Literatura"]),
                            Tamanho = (string)(dr["Tamanho"]),
                            Quantidade = (int)(dr["Quantidade"]),
                            Capacidade = (int)(dr["capacidadeEstoque"]),
                            Generos = dr["Generos"] == DBNull.Value ? new List<string>() : dr["Generos"].ToString().Split(", ").ToList(),
                            PromocaoPorcentagem = (int)(dr["PromocaoPorcentagem"])
                        },
                        dr["Imagem"].ToString(),
                        dr["ArquivoEbook"].ToString(),
                        Convert.ToDecimal(dr["MediaAvaliacao"]),
                        Convert.ToInt32(dr["NumeroVendas"])
                    ));
                }

                return listaLivro;
            }
        }

        public IEnumerable<(Livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> ObterMaisVendidos()
        {
            List<(Livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)> listaLivro = new List<(Livro, string arquivoImagem, string arquivoEbook, decimal mediaLivro, int numeroVendas)>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand consulta = new MySqlCommand(
                    "select li.*, vv.NumeroVendas, va.MediaAvaliacao from ViewLivro li inner join ViewLivroVendas vv on li.codBarras = vv.codBarras inner join ViewLivroAvaliacoes va on li.codBarras = va.codBarras where(Quantidade > 0) order by vv.NumeroVendas desc limit 15",
                    conexao
                );

                if (_loginFuncionario.GetFuncionario() != null)
                {
                    consulta = new MySqlCommand(
                        "select li.*, vv.NumeroVendas, va.MediaAvaliacao from ViewLivro li inner join ViewLivroVendas vv on li.codBarras = vv.codBarras inner join ViewLivroAvaliacoes va on li.codBarras = va.codBarras order by vv.NumeroVendas desc limit 15",
                        conexao
                    );
                }

                MySqlDataAdapter da = new MySqlDataAdapter(consulta);
                DataTable dt = new DataTable();

                da.Fill(dt);

                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    listaLivro.Add((
                        new Livro
                        {
                            CodigoBarras = (decimal)(dr["codBarras"]),
                            ISBN = Convert.ToUInt64(dr["ISBN"]).ToString(@"000-0-00-000000-0"),
                            Nome = (string)(dr["Nome"]),
                            Preco = Convert.ToDecimal(dr["Preco"]).ToString("N2", CultureInfo.GetCultureInfo("pt-BR")),
                            Sinopse = (string)(dr["Sinopse"]),
                            Autor = (string)(dr["Autor"]),
                            Editora = (string)(dr["Editora"]),
                            IdadeRecomendada = (string)(dr["IdadeMinRecomendada"]),
                            DataPubli = DateOnly.FromDateTime(Convert.ToDateTime(dr["DataPublicacao"])),
                            TipoCapa = (string)(dr["Tipo"]),
                            Idioma = (string)(dr["Idioma"]),
                            Literatura = (string)(dr["Literatura"]),
                            Tamanho = (string)(dr["Tamanho"]),
                            Quantidade = (int)(dr["Quantidade"]),
                            Capacidade = (int)(dr["capacidadeEstoque"]),
                            Generos = dr["Generos"] == DBNull.Value ? new List<string>() : dr["Generos"].ToString().Split(", ").ToList(),
                            PromocaoPorcentagem = (int)(dr["PromocaoPorcentagem"])
                        },
                        dr["Imagem"].ToString(),
                        dr["ArquivoEbook"].ToString(),
                        Convert.ToDecimal(dr["MediaAvaliacao"]),
                        Convert.ToInt32(dr["NumeroVendas"])
                    ));
                }

                return listaLivro;
            }
        }

        public bool EmAlta(string codBarras)
        {
            using(var conexao = new MySqlConnection(_conexaoMySQL))
            {
                bool existencia;
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from (select * from ViewLivroVendas order by NumeroVendas desc limit 15) as MaisVendidos where(codBarras = @CodBarras);", conexao);
                cmd.Parameters.AddWithValue("@CodBarras", Convert.ToDecimal(codBarras));
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                existencia = dr.Read();

                return existencia;
            }
        }  

    }
}