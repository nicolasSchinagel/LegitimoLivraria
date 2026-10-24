namespace LivrariaLegitimo.Cookie
{
    public class Cookie
    {
        private IHttpContextAccessor _context;
        private IConfiguration _configuration;
        public Cookie(IHttpContextAccessor context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        // parei aqui 

        public void Cadastrar(string Key, string Valor)
        {
            CookieOptions Options = new CookieOptions();
            Options.Expires = DateTime.Now.AddDays(7);
            Options.IsEssential = true;

            _context.HttpContext.Response.Cookies.Append(Key, Valor, Options);
        }
        public void Atualizar(string Key, string Valor)
        {
            if(Existe(Key))
            {
                Remover(Key);
            }
            Cadastrar(Key, Valor);
        }
        public void Remover(string Key)
        {
            var valor = _context.HttpContext.Request.Cookies[Key];
        }
        public string Consultar(string Key, bool Cript = true)
        {
            var valor = _context.HttpContext.Request.Cookies[Key];
            return valor;
        }
        public bool Existe(string Key)
        {
            if (_context.HttpContext.Request.Cookies[Key] == null)
            {
                return false;
            }
            return true;
        }


    }
}
