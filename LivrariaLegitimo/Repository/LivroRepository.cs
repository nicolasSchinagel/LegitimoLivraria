using LivrariaLegitimo.GerenciaArquivos;
using LivrariaLegitimo.Models;
using LivrariaLegitimo.Repository.Contract;
using MySql.Data.MySqlClient;
using System.Globalization;
using X.PagedList;
using X.PagedList.Extensions;

namespace LivrariaLegitimo.Repository
{
    public class LivroRepository : ILivroRepository
    {
        private readonly string _conexaoMySQL;
        IConfiguration _config;




        public IEnumerable<Livro> ObterTodosLivros()
        {
            var lista = new List<Livro>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from ViewLivro order by Nome", conexao);

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        lista.Add(MapearLivro(dr));
                }
            }
            return lista;
        }

        // ---------- MELHORES AVALIADOS ----------
        public IEnumerable<Livro> ObterMelhoresAvaliados()
        {
            var lista = new List<Livro>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand(
                    @"select l.* from ViewLivro l
              inner join Avaliacao a on a.CodigoBarras = l.CodigoBarras
              group by l.CodigoBarras
              order by avg(a.NotaLivro) desc, l.Nome", conexao);

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        lista.Add(MapearLivro(dr));
                }
            }
            return lista;
        }

        public IPagedList<Livro> ObterMelhoresAvaliados(int? pagina, string pesquisa)
        {
            IEnumerable<Livro> livros = ObterMelhoresAvaliados();

            if (!string.IsNullOrWhiteSpace(pesquisa))
            {
                string p = pesquisa.Trim();
                livros = livros.Where(l =>
                    (l.Nome ?? "").Contains(p, StringComparison.OrdinalIgnoreCase) ||
                    (l.Autor ?? "").Contains(p, StringComparison.OrdinalIgnoreCase) ||
                    (l.Editora ?? "").Contains(p, StringComparison.OrdinalIgnoreCase) ||
                    (l.Literatura ?? "").Contains(p, StringComparison.OrdinalIgnoreCase));
            }

            return livros.ToPagedList(pagina ?? 1, 5);
        }

        // ---------- MAIS VENDIDOS ----------
        public IEnumerable<Livro> ObterMaisVendidos()
        {
            var lista = new List<Livro>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand(
                    @"select l.* from ViewLivro l
              inner join ItemVenda iv on iv.CodBarras = l.CodigoBarras
              group by l.CodigoBarras
              order by sum(iv.QtdItem) desc, l.Nome", conexao);

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        lista.Add(MapearLivro(dr));
                }
            }
            return lista;
        }
        public IPagedList<Livro> ObterMaisVendidos(int? pagina)
        {
            IEnumerable<Livro> livros = ObterMaisVendidos();

            return livros.ToPagedList(pagina ?? 1, 5);
        }

        // ---------- POR LITERATURA ----------
        public IEnumerable<Livro> ObterPorLiteratura(string literatura)
        {
            var lista = new List<Livro>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "select * from ViewLivro where Literatura = @Literatura order by Nome", conexao);
                cmd.Parameters.AddWithValue("@Literatura", literatura);

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        lista.Add(MapearLivro(dr));
                }
            }
            return lista;
        }

        // ---------- BARRA DE PESQUISA ----------
        public IEnumerable<Livro> ObterPorPesquisa(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return ObterTodosLivros();

            var lista = new List<Livro>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand(
                    @"select * from ViewLivro
              where Nome like @Pesquisa
                 or Autor like @Pesquisa
                 or Editora like @Pesquisa
                 or Literatura like @Pesquisa
              order by Nome", conexao);
                cmd.Parameters.AddWithValue("@Pesquisa", "%" + nome.Trim() + "%");

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        lista.Add(MapearLivro(dr));
                }
            }
            return lista;
        }

        // ---------- HISTORICO GERAL (G) ----------
        public IEnumerable<Livro> ObterPorHistoricoG(List<string> codBarrasG)
        {
            var lista = new List<Livro>();
            if (codBarrasG == null || codBarrasG.Count == 0)
                return lista;

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                // monta @c0, @c1, @c2...
                var nomes = new List<string>();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                for (int i = 0; i < codBarrasG.Count; i++)
                {
                    nomes.Add("@c" + i);
                    cmd.Parameters.Add("@c" + i, MySqlDbType.Decimal).Value =
                        decimal.Parse(codBarrasG[i], CultureInfo.InvariantCulture);
                }
                cmd.CommandText = "select * from ViewLivro where CodigoBarras in (" + string.Join(",", nomes) + ")";

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        lista.Add(MapearLivro(dr));
                }
            }

            // mantem a ordem do historico
            return lista.OrderBy(l => codBarrasG.IndexOf(l.CodigoBarras.ToString(CultureInfo.InvariantCulture))).ToList();
        }

        // ---------- HISTORICO DE EBOOKS (E) ----------
        public IEnumerable<Livro> ObterPorHistoricoE(List<string> codBarrasE)
        {
            var lista = new List<Livro>();
            if (codBarrasE == null || codBarrasE.Count == 0)
                return lista;

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                var nomes = new List<string>();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexao;
                for (int i = 0; i < codBarrasE.Count; i++)
                {
                    nomes.Add("@c" + i);
                    cmd.Parameters.Add("@c" + i, MySqlDbType.Decimal).Value =
                        decimal.Parse(codBarrasE[i], CultureInfo.InvariantCulture);
                }
                cmd.CommandText = "select * from ViewLivro where CodigoBarras in (" + string.Join(",", nomes) + ")";

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        lista.Add(MapearLivro(dr));
                }
            }

            return lista.OrderBy(l => codBarrasE.IndexOf(l.CodigoBarras.ToString(CultureInfo.InvariantCulture))).ToList();
        }

        // ---------- converte uma linha do banco em Livro ----------
        private Livro MapearLivro(MySqlDataReader dr)
        {
            string isbn = Convert.ToDecimal(dr["ISBN"]).ToString("0", CultureInfo.InvariantCulture);

            if (isbn.Length == 13)
                isbn = $"{isbn[..3]}-{isbn.Substring(3, 2)}-{isbn.Substring(5, 4)}-{isbn.Substring(9, 3)}-{isbn[12]}";

            return new Livro
            {
                CodigoBarras = Convert.ToDecimal(dr["CodigoBarras"]),
                Nome = Convert.ToString(dr["Nome"]),
                Preco = Convert.ToDecimal(dr["Preco"]).ToString("F2", CultureInfo.GetCultureInfo("pt-BR")),
                Autor = Convert.ToString(dr["Autor"]),
                Editora = Convert.ToString(dr["Editora"]),
                Promocao = Convert.ToInt32(dr["Promocao"]),
                Generos = Convert.ToString(dr["Generos"])
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .ToList(),
                IdadeRecomendada = Convert.ToString(dr["IdadeRecomendada"]),
                Sinopse = Convert.ToString(dr["Sinopse"]),
                DataPubli = DateOnly.FromDateTime(Convert.ToDateTime(dr["DataPubli"])),
                TipoCapa = Convert.ToString(dr["TipoCapa"]),
                Idioma = Convert.ToString(dr["Idioma"]),
                Tamanho = Convert.ToString(dr["Tamanho"]),
                Capacidade = Convert.ToInt32(dr["Capacidade"]),
                Quantidade = Convert.ToInt32(dr["Quantidade"]),
                ISBN = isbn,
                Literatura = Convert.ToString(dr["Literatura"]),
                arquivoImagem = dr["ImagemPri"] == DBNull.Value ? null : Convert.ToString(dr["ImagemPri"]),
                arquivoEbook = dr["ArquivoEbook"] == DBNull.Value ? null : Convert.ToString(dr["ArquivoEbook"])
            };
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
    }
}