using ClinicaMedica.Web.Configuration;
using ClinicaMedica.Web.Data;
using ClinicaMedica.Web.Daos;
using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.Infrastructure.Repositories;
using ClinicaMedica.Web.Services;
using ClinicaMedica.Web.Services.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using ClinicaMedica.Web.Core.Entities;
using ClinicaMedica.Web.Infrastructure.Database.Providers;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// ========================
// Configurações
// ========================
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));

// ========================
// Infrastructure
// ========================
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IDatabaseProviderResolver, DatabaseProviderResolver>();
builder.Services.AddScoped<IDbConnectionProvider, MySqlConnectionProvider>();
builder.Services.AddScoped<IDbConnectionProvider, OracleConnectionProvider>();
builder.Services.AddScoped<DbConnectionFactory>();


// ========================
// Repositories
// ========================
builder.Services.AddScoped<IGenericRepository<Medico>, MedicoRepository>();

// ========================
// DAOs (Mantém compatibilidade)
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
// Services de Configuração
// ========================
IServiceCollection serviceCollection = builder.Services.AddScoped<IDatabaseConfigurationService, DatabaseConfigurationService>();

// ========================
// MVC & Pages
// ========================
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

// ========================
// CORS
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
// DataProtection
// ========================
var keysFolder = @"C:\Projetos\ClinicaMedica\publish\Keys";
Directory.CreateDirectory(keysFolder);
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
    .SetApplicationName("ClinicaMedica");

var app = builder.Build();

// ========================
// Middleware
// ========================
var ptBR = new CultureInfo("pt-BR");
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(ptBR),
    SupportedCultures = new List<CultureInfo> { ptBR },
    SupportedUICultures = new List<CultureInfo> { ptBR }
};

if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRequestLocalization(localizationOptions);
app.UseRouting();
app.UseCors("AllowConsumer");
app.UseAuthorization();

// ========================
// Rotas
// ========================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.MapControllers();

app.Run();
