using LivrariaLegitimo.Repository.Contract;
using LivrariaLegitimo.Repository;
//using LivrariaLegitimo.Libraries.Middleware;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClientePFRepository, ClientePFRepository>();
builder.Services.AddScoped<IClientePJRepository, ClientePJRepository>();
builder.Services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
builder.Services.AddScoped<LivrariaLegitimo.Libraries.Sessao.Sessao>();
builder.Services.AddScoped<LivrariaLegitimo.Libraries.Login.LoginCliente>();
builder.Services.AddScoped<LivrariaLegitimo.Libraries.Login.LoginFuncionario>();
// colocar outros serviços aqui depois

// Corrigir problema com TEMPDATA
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    // Definir um tempo para duração. 
    options.IdleTimeout = TimeSpan.FromSeconds(900);
    options.Cookie.HttpOnly = true;
    // Mostrar para o navegador que o cookie e essencial   
    options.Cookie.IsEssential = true;
});
builder.Services.AddMvc().AddSessionStateTempDataProvider();

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Default/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapStaticAssets();

// colocar outros aqui depois
app.UseSession();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Default}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
