using ClinicaMedica.Consumidor.Infrastructure.Http;
using ClinicaMedica.Consumidor.Services;
using ClinicaMedica.Consumidor.Services.Interfaces;
using ClinicaMedica.Consumidor.Filters;

var builder = WebApplication.CreateBuilder(args);

#region 🔧 SERVICES

// MVC + Filtro global de erro
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});

// HttpContext (necessário para Session e IUserContext)
builder.Services.AddHttpContextAccessor();

// Session (estado do usuário - escolha do banco)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Contexto do usuário (abstração da Session)
builder.Services.AddScoped<IUserContext, UserContext>();

// DelegatingHandler (injeta X-Database automaticamente)
builder.Services.AddTransient<DatabaseDelegatingHandler>();

// HttpClient + ApiService (sem lógica de banco)
builder.Services.AddHttpClient<IApiService, ApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:5001/");
});
builder.Services.AddScoped<IMedicoService, MedicoService>();
builder.Services.AddScoped<DatabaseFilter>();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<DatabaseFilter>();
});

#endregion

var app = builder.Build();

#region 🌐 PIPELINE

// Tratamento de erro global (produção)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Session precisa vir antes de endpoints
app.UseSession();

// (Opcional) Autenticação futura
// app.UseAuthentication();
// app.UseAuthorization();

#endregion

#region 🛣️ ROTAS

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

#endregion

app.Run();