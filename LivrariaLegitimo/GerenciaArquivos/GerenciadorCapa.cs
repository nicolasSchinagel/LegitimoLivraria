namespace LivrariaLegitimo.GerenciaArquivos
{
    public class GerenciadorCapa
    {
        public static string CadastrarCapaLivro(IFormFile file)
        {
            var NomeArquivo = Path.GetFileName(file.FileName);
            var Caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/capas", NomeArquivo);

            using (var stream = new FileStream(Caminho, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return Path.Combine("/capas", NomeArquivo).Replace("\\", "/");
        }
    }
}
