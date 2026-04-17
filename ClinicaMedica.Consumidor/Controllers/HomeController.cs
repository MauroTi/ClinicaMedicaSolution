using ClinicaMedica.Consumidor.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedica.Consumidor.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApiService _apiService;

        public HomeController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var database = HttpContext.Session.GetString("database") ?? "mysql";
                _apiService.SetDatabase(database);

                var totalMedicos = await _apiService.GetAllAsync<MedicoViewModel>("medicosApi");
                var totalPacientes = await _apiService.GetAllAsync<PacienteViewModel>("pacientes");
                var totalConsultas = await _apiService.GetAllAsync<ConsultaViewModel>("consultas");

                var model = new HomeViewModel
                {
                    TotalMedicos = totalMedicos.Count,
                    TotalPacientes = totalPacientes.Count,
                    TotalConsultas = totalConsultas.Count
                };

                return View(model);
            }
            catch (Exception ex)
            {
                ViewData["ErroApi"] = ex.Message;

                return View(new HomeViewModel
                {
                    TotalMedicos = 0,
                    TotalPacientes = 0,
                    TotalConsultas = 0
                });
            }
        }
    }
}
