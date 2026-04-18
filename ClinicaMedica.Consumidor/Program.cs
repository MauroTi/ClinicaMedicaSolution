using ClinicaMedica.Consumidor.Filters;
using ClinicaMedica.Consumidor.Infrastructure.Http;
using ClinicaMedica.Consumidor.Services;
using ClinicaMedica.Consumidor.Services.Implementations;
using ClinicaMedica.Consumidor.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
    options.Filters.Add<DatabaseFilter>();
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<GlobalExceptionFilter>();
builder.Services.AddScoped<DatabaseFilter>();
builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddTransient<DatabaseDelegatingHandler>();

var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:5001/";

builder.Services.AddHttpClient<IApiService, ApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl, UriKind.Absolute);
})
.AddHttpMessageHandler<DatabaseDelegatingHandler>();

builder.Services.AddScoped<IMedicoService, MedicoService>();
builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<IConsultaService, ConsultaService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
