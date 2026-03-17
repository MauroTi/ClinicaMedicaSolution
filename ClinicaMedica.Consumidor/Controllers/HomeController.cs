using Microsoft.AspNetCore.Mvc;
using ClinicaMedica.Consumidor.Services;
using ClinicaMedica.Consumidor.ViewModels;

namespace ClinicaMedica.Consumidor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiService _apiService;

        public HomeController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // AJUSTE ESTES ENDPOINTS CONFORME A SUA API REAL
                var totalMedicos = await _apiService.GetAllAsync<object>("api/medicosapi");
                var totalPacientes = await _apiService.GetAllAsync<object>("api/Pacientes");
                var totalConsultas = await _apiService.GetAllAsync<object>("api/Consultas");

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
                var model = new HomeViewModel
                {
                    TotalMedicos = 0,
                    TotalPacientes = 0,
                    TotalConsultas = 0
                };

                ViewData["ErroApi"] = ex.Message;
                return View(model);
            }
        }
    }
}