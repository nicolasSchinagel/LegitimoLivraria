namespace LivrariaLegitimo.GerenciaArquivos
{
    public class GerenciadorEbook
    {
        public static string CadastrarArquivoEbook(IFormFile file)
        {
            var NomeArquivo = Path.GetFileName(file.FileName);
            var Caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ebooks", NomeArquivo);

            using (var stream = new FileStream(Caminho, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return Path.Combine("/ebooks", NomeArquivo).Replace("\\", "/");
        }
    }
}
