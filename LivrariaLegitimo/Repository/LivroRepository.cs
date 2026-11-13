using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository.Contract;
using MySql.Data.MySqlClient;
using X.PagedList;
using X.PagedList.Extensions;
using LivrariaLegitimo.GerenciaArquivos;

namespace LivrariaLegitimo.Repository
{
    public class LivroRepository : ILivroRepository
    {
        private readonly string _conexaoMySQL;
        IConfiguration _config;

        // Quantidade de livros por página nas telas paginadas
        private const int RegistrosPorPagina = 12;

        public LivroRepository(IConfiguration conf)
        {
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
            _config = conf;
        }

        
        //  METODOS DE SELECT (para as telas e a barra de pesquisa)
        

        // ---------- TODOS OS LIVROS ----------
        public IEnumerable<Livro> ObterTodosLivros()
        {
            return Selecionar("select * from ViewLivro order by Nome", null);
        }

        public IPagedList<Livro> ObterTodosLivros(int? pagina, string pesquisa)
        {
            // Se vier texto da barra de pesquisa, filtra; se não, traz tudo
            IEnumerable<Livro> livros = string.IsNullOrWhiteSpace(pesquisa)
                ? ObterTodosLivros()
                : ObterPorPesquisa(pesquisa);

            return livros.ToPagedList(pagina ?? 1, RegistrosPorPagina);
        }

        // ---------- MELHORES AVALIADOS ----------
        public IEnumerable<Livro> ObterMelhoresAvaliados()
        {
            string sql = @"select l.* from ViewLivro l
                           inner join Avaliacao a on a.CodigoBarras = l.CodigoBarras
                           group by l.CodigoBarras
                           order by avg(a.NotaLivro) desc, l.Nome";
            return Selecionar(sql, null);
        }

        public IPagedList<Livro> ObterMelhoresAvaliados(int? pagina, string pesquisa)
        {
            IEnumerable<Livro> livros = ObterMelhoresAvaliados();
            livros = FiltrarPorTexto(livros, pesquisa);
            return livros.ToPagedList(pagina ?? 1, RegistrosPorPagina);
        }

        // ---------- MAIS VENDIDOS ----------
        public IEnumerable<Livro> ObterMaisVendidos()
        {
            string sql = @"select l.* from ViewLivro l
                           inner join ItemVenda iv on iv.CodBarras = l.CodigoBarras
                           group by l.CodigoBarras
                           order by sum(iv.QtdItem) desc, l.Nome";
            return Selecionar(sql, null);
        }

        public IPagedList<Livro> ObterMaisVendidos(int? pagina, string pesquisa)
        {
            IEnumerable<Livro> livros = ObterMaisVendidos();
            livros = FiltrarPorTexto(livros, pesquisa);
            return livros.ToPagedList(pagina ?? 1, RegistrosPorPagina);
        }

        // ---------- POR LITERATURA ----------
        public IEnumerable<Livro> ObterPorLiteratura(string literatura)
        {
            var parametros = new List<MySqlParameter>
            {
                new MySqlParameter("@Literatura", MySqlDbType.VarChar) { Value = literatura }
            };
            return Selecionar("select * from ViewLivro where Literatura = @Literatura order by Nome", parametros);
        }

        // Aqui o parametro "pesquisa" carrega o nome da literatura escolhida
        public IPagedList<Livro> ObterPorLiteratura(int? pagina, string pesquisa)
        {
            return ObterPorLiteratura(pesquisa).ToPagedList(pagina ?? 1, RegistrosPorPagina);
        }

        // ---------- BARRA DE PESQUISA ----------
        public IEnumerable<Livro> ObterPorPesquisa(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return ObterTodosLivros();

            string sql = @"select * from ViewLivro
                           where Nome like @Pesquisa
                              or Autor like @Pesquisa
                              or Editora like @Pesquisa
                              or Literatura like @Pesquisa
                           order by Nome";

            var parametros = new List<MySqlParameter>
            {
                new MySqlParameter("@Pesquisa", MySqlDbType.VarChar) { Value = "%" + nome.Trim() + "%" }
            };
            return Selecionar(sql, parametros);
        }

        public IPagedList<Livro> ObterPorPesquisa(int? pagina, string pesquisa)
        {
            return ObterPorPesquisa(pesquisa).ToPagedList(pagina ?? 1, RegistrosPorPagina);
        }

        // ---------- HISTORICO GERAL (G) ----------
        public IEnumerable<Livro> ObterPorHistoricoG(List<string> codBarrasG)
        {
            return SelecionarPorCodigos(codBarrasG);
        }

        // Aqui "pesquisa" vem com os codigos de barras separados por virgula
        public IPagedList<Livro> ObterPorHistoricoG(int? pagina, string pesquisa)
        {
            return SelecionarPorCodigos(SepararCodigos(pesquisa)).ToPagedList(pagina ?? 1, RegistrosPorPagina);
        }

        // ---------- HISTORICO DE EBOOKS (E) ----------
        public IEnumerable<Livro> ObterPorHistoricoE(List<string> codBarrasE)
        {
            return SelecionarPorCodigos(codBarrasE);
        }

        // Aqui "pesquisa" vem com os codigos de barras separados por virgula
        public IPagedList<Livro> ObterPorHistoricoE(int? pagina, string pesquisa)
        {
            return SelecionarPorCodigos(SepararCodigos(pesquisa)).ToPagedList(pagina ?? 1, RegistrosPorPagina);
        }

        
        //  AUXILIARES (usados pelos metodos acima)
        

        private IEnumerable<Livro> SelecionarPorCodigos(List<string> codigos)
        {
            if (codigos == null || codigos.Count == 0)
                return new List<Livro>();

            // Monta @c0, @c1, @c2... para nao concatenar valor direto no SQL
            var parametros = new List<MySqlParameter>();
            var nomes = new List<string>();
            for (int i = 0; i < codigos.Count; i++)
            {
                string nomeParam = "@c" + i;
                nomes.Add(nomeParam);
                parametros.Add(new MySqlParameter(nomeParam, MySqlDbType.VarChar) { Value = codigos[i] });
            }

            string sql = "select * from ViewLivro where CodigoBarras in (" + string.Join(",", nomes) + ")";
            var livros = Selecionar(sql, parametros).ToList();

            // Mantem a ordem do historico (mais recente primeiro, como veio na lista)
            return livros.OrderBy(l => codigos.IndexOf(l.CodigoBarras.ToString()));
        }

        private static List<string> SepararCodigos(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return new List<string>();

            return texto.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
        }

        private static IEnumerable<Livro> FiltrarPorTexto(IEnumerable<Livro> livros, string pesquisa)
        {
            if (string.IsNullOrWhiteSpace(pesquisa))
                return livros;

            string p = pesquisa.Trim();
            return livros.Where(l =>
                (l.Nome ?? "").Contains(p, StringComparison.OrdinalIgnoreCase) ||
                (l.Autor ?? "").Contains(p, StringComparison.OrdinalIgnoreCase) ||
                (l.Editora ?? "").Contains(p, StringComparison.OrdinalIgnoreCase) ||
                (l.Literatura ?? "").Contains(p, StringComparison.OrdinalIgnoreCase));
        }
        
        private List<Livro> Selecionar(string sql, List<MySqlParameter> parametros)
            
        {
            /*
            var lista = new List<Livro>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conexao);

                if (parametros != null)
                {
                    foreach (var p in parametros)
                        cmd.Parameters.Add(p);
                }

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Livro
                        {
                            CodigoBarras = Convert.ToInt32(dr["CodigoBarras"]),
                            Nome = Convert.ToString(dr["Nome"]),
                            Preco = Convert.ToDecimal(dr["Preco"]),
                            Autor = Convert.ToString(dr["Autor"]),
                            Editora = Convert.ToString(dr["Editora"]),
                            Promocao = Convert.ToInt32(dr["Promocao"]),
                            // Generos vem como texto separado por virgula: "Fantasia,Aventura"
                            Generos = Convert.ToString(dr["Generos"])
                                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                                .ToList(),
                            IdadeRecomendada = Convert.ToString(dr["IdadeRecomendada"]),
                            Sinopse = Convert.ToString(dr["Sinopse"]),
                            DataPubli = DateOnly.FromDateTime(Convert.ToDateTime(dr["DataPubli"])),
                            TipoCapa = Convert.ToString(dr["TipoCapa"]),
                            Idioma = Convert.ToString(dr["Idioma"]),
                            Tamanho = Convert.ToDecimal(dr["Tamanho"]),
                            Capacidade = Convert.ToInt32(dr["Capacidade"]),
                            Quantidade = Convert.ToInt32(dr["Quantidade"]),
                            ISBN = Convert.ToInt32(dr["ISBN"]),
                            Literatura = Convert.ToString(dr["Literatura"]),
                            Imagem = Convert.ToString(dr["Imagem"]),
                            ArquivoEbook = dr["ArquivoEbook"] == DBNull.Value ? null : Convert.ToString(dr["ArquivoEbook"])
                        });
                    }
                }
            }

            return lista;
            */
            throw new NotImplementedException();
        }

        
        //  NAO SAO SELECTS - ignorar / manter o que ja tem
        //  (so estao aqui pra classe compilar implementando ILivroRepository)
        


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
                cmd.Parameters.Add("@Preco", MySqlDbType.Decimal).Value = Convert.ToDecimal(livro.Preco.Replace(",", "."));
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
    }
}