
using ClinicaMedica.Web.Daos;
using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.Data;
using ClinicaMedica.Web.Interfaces;
using ClinicaMedica.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DbConnectionFactory
builder.Services.AddSingleton<DbConnectionFactory>();
builder.Services.AddScoped<IDashboardDao, DashboardDao>();

// DAOs
builder.Services.AddScoped<IMedicoDao, MedicoDao>();
builder.Services.AddScoped<IPacienteDao, PacienteDao>();
builder.Services.AddScoped<IConsultaDao, ConsultaDao>();
builder.Services.AddScoped<IDashboardDao, DashboardDao>();
// Services
builder.Services.AddScoped<MedicoService>();
builder.Services.AddScoped<PacienteService>();
builder.Services.AddScoped<ConsultaService>();
builder.Services.AddScoped<DashboardService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// MVC default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// API controllers
app.MapControllers();

app.Run();