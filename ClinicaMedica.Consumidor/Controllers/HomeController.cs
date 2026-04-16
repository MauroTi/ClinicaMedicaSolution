using Microsoft.AspNetCore.Mvc;
using ClinicaMedica.Consumidor.ViewModels;
using ClinicaMedica.Consumidor.Services.Interfaces;

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
                var totalMedicos = await _apiService.GetAllAsync<MedicoViewModel>("api/medicosApi");
                var totalPacientes = await _apiService.GetAllAsync<PacienteViewModel>("api/pacientes");
                var totalConsultas = await _apiService.GetAllAsync<ConsultaViewModel>("api/consultas");

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