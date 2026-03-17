using Microsoft.AspNetCore.Mvc;
using ClinicaMedica.Consumidor.Services;
using ClinicaMedica.Consumidor.Models;

namespace ClinicaMedica.Consumidor.Controllers
{
    public class PacientesController : Controller
    {
        private readonly ApiService _apiService;

        public PacientesController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var pacientes = await _apiService.GetAllAsync<Paciente>("api/Pacientes");
                return View(pacientes);
            }
            catch (Exception ex)
            {
                ViewData["ErroApi"] = ex.Message;
                return View(new List<Paciente>());
            }
        }
    }
}