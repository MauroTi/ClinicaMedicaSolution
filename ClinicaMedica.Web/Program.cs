using ClinicaMedica.Web.Configuration;
using ClinicaMedica.Web.Core.Entities;
using ClinicaMedica.Web.Daos;
using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.Data;
using ClinicaMedica.Web.Data.Dialects;
using ClinicaMedica.Web.Infrastructure.Database.Providers;
using ClinicaMedica.Web.Infrastructure.Repositories;
using ClinicaMedica.Web.Services;
using ClinicaMedica.Web.Services.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// ========================
// Configurações
// ========================
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));

// ========================
// Infraestrutura
// ========================
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IDatabaseProviderResolver, DatabaseProviderResolver>();

builder.Services.AddScoped<IDbConnectionProvider, MySqlConnectionProvider>();
builder.Services.AddScoped<IDbConnectionProvider, OracleConnectionProvider>();

builder.Services.AddScoped<DbConnectionFactory>();

// 🔥 CORRETO: evitar duplicação
builder.Services.AddScoped<DialectFactory>();

// ========================
// Repositories
// ========================
builder.Services.AddScoped<IGenericRepository<Medico>, MedicoRepository>();

// ========================
// DAOs
// ========================
builder.Services.AddScoped<IDashboardDao, DashboardDao>();
builder.Services.AddScoped<IMedicoDao, MedicoDao>();
builder.Services.AddScoped<IPacienteDao, PacienteDao>();
builder.Services.AddScoped<IConsultaDao, ConsultaDao>();

// ========================
// Services (negócio)
// ========================
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<MedicoService>();
builder.Services.AddScoped<PacienteService>();
builder.Services.AddScoped<ConsultaService>();

builder.Services.AddScoped<IMedicoService, MedicoService>();
builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<IConsultaService, ConsultaService>();

// ========================
// Config Service
// ========================
builder.Services.AddScoped<IDatabaseConfigurationService, DatabaseConfigurationService>();

// ========================
// MVC
// ========================
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// ========================
// CORS
// ========================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowConsumer", policy =>
    {
        policy
            .WithOrigins("http://localhost:8082")
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

// ========================
// BUILD APP
// ========================
var app = builder.Build();

// ========================
// Middleware
// ========================
var ptBR = new CultureInfo("pt-BR");

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(ptBR),
    SupportedCultures = new List<CultureInfo> { ptBR },
    SupportedUICultures = new List<CultureInfo> { ptBR }
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowConsumer");

app.UseAuthorization();

// ========================
// ROTAS
// ========================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.MapControllers();

app.Run();