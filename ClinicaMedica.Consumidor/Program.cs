using ClinicaMedica.Consumidor.Services;

var builder = WebApplication.CreateBuilder(args);

// Adiciona suporte a Controllers + Views
builder.Services.AddControllersWithViews();

// Registra o ApiService com HttpClient
builder.Services.AddHttpClient<ApiService>(client =>
{
    // ⚠️ TROQUE A PORTA SE SUA API ESTIVER EM OUTRA
    client.BaseAddress = new Uri("https://localhost:5001/");
    client.Timeout = TimeSpan.FromSeconds(30);
});

// ✅ Registra o MedicoService
builder.Services.AddScoped<MedicoService>();
builder.Services.AddScoped<PacienteService>();
builder.Services.AddScoped<ConsultaService>();

var app = builder.Build();

// Pipeline de tratamento de erros
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Middlewares padrão
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Rota padrão MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();