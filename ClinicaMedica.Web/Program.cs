using ClinicaMedica.Web.Daos;
using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.Data;
using ClinicaMedica.Web.Services;
using ClinicaMedica.Web.Services.Interfaces;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DbConnectionFactory
builder.Services.AddSingleton<DbConnectionFactory>();

// DAOs
builder.Services.AddScoped<IDashboardDao, DashboardDao>();
builder.Services.AddScoped<IMedicoDao, MedicoDao>();
builder.Services.AddScoped<IPacienteDao, PacienteDao>();
builder.Services.AddScoped<IConsultaDao, ConsultaDao>();

// Services
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<MedicoService>();
builder.Services.AddScoped<PacienteService>();
builder.Services.AddScoped<ConsultaService>();

builder.Services.AddScoped<IMedicoService, MedicoService>();
builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<IConsultaService, ConsultaService>();

var app = builder.Build();

// Configuração de cultura pt-BR
var ptBR = new CultureInfo("pt-BR");

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(ptBR),
    SupportedCultures = new List<CultureInfo> { ptBR },
    SupportedUICultures = new List<CultureInfo> { ptBR }
};

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// IMPORTANTE: deve vir antes do UseRouting
app.UseRequestLocalization(localizationOptions);

app.UseRouting();

app.UseAuthorization();

// MVC default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// API controllers
app.MapControllers();

app.Run();