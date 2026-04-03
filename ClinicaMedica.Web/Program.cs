using ClinicaMedica.Web.Daos;
using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.Data;
using ClinicaMedica.Web.Services;
using ClinicaMedica.Web.Services.Interfaces;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using System.IO;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// ========================
// Serviços MVC e Controllers
// ========================
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

// ========================
// DB Connection Factory
// ========================
builder.Services.AddSingleton<DbConnectionFactory>();

// ========================
// DAOs
// ========================
builder.Services.AddScoped<IDashboardDao, DashboardDao>();
builder.Services.AddScoped<IMedicoDao, MedicoDao>();
builder.Services.AddScoped<IPacienteDao, PacienteDao>();
builder.Services.AddScoped<IConsultaDao, ConsultaDao>();

// ========================
// Services
// ========================
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<MedicoService>();
builder.Services.AddScoped<PacienteService>();
builder.Services.AddScoped<ConsultaService>();

builder.Services.AddScoped<IMedicoService, MedicoService>();
builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<IConsultaService, ConsultaService>();

// ========================
// CORS - permitir Consumer na porta 8082
// ========================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowConsumer", policy =>
    {
        policy.WithOrigins("http://localhost:8082")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ========================
// DataProtection - persistir chaves em pasta para IIS
// ========================
var keysFolder = @"C:\Projetos\ClinicaMedica\publish\Keys";
Directory.CreateDirectory(keysFolder); // garante que pasta exista
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
    .SetApplicationName("ClinicaMedica");

// ========================
// Construir app
// ========================
var app = builder.Build();

// ========================
// Cultura pt-BR
// ========================
var ptBR = new CultureInfo("pt-BR");
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(ptBR),
    SupportedCultures = new List<CultureInfo> { ptBR },
    SupportedUICultures = new List<CultureInfo> { ptBR }
};

// ========================
// Pipeline HTTP
// ========================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Sem HTTPS no IIS
// app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRequestLocalization(localizationOptions);

app.UseRouting();

// Habilitar CORS
app.UseCors("AllowConsumer");

app.UseAuthorization();

// MVC default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// API controllers
app.MapControllers();

app.Run();