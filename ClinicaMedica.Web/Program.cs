using ClinicaMedica.Web.Configuration;
using ClinicaMedica.Web.Daos;
using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.Data;
using ClinicaMedica.Web.Data.Dialects;
using ClinicaMedica.Web.Infrastructure.Database.Providers;
using ClinicaMedica.Web.Services;
using ClinicaMedica.Web.Services.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IDatabaseProviderResolver, DatabaseProviderResolver>();
builder.Services.AddScoped<IDbConnectionProvider, MySqlConnectionProvider>();
builder.Services.AddScoped<IDbConnectionProvider, OracleConnectionProvider>();
builder.Services.AddScoped<DbConnectionFactory>();
builder.Services.AddScoped<DialectFactory>();

builder.Services.AddScoped<IDashboardDao, DashboardDao>();
builder.Services.AddScoped<IMedicoDao, MedicoDao>();
builder.Services.AddScoped<IPacienteDao, PacienteDao>();
builder.Services.AddScoped<IConsultaDao, ConsultaDao>();

builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IMedicoService, MedicoService>();
builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<IConsultaService, ConsultaService>();
builder.Services.AddScoped<IDatabaseConfigurationService, DatabaseConfigurationService>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

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

var keysFolder = Path.Combine(builder.Environment.ContentRootPath, "App_Data", "Keys");
Directory.CreateDirectory(keysFolder);

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysFolder))
    .SetApplicationName("ClinicaMedica");

var app = builder.Build();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.MapControllers();

app.Run();
