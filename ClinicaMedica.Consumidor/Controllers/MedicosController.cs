using Microsoft.AspNetCore.Mvc;
using ClinicaMedica.Consumidor.Services;
using ClinicaMedica.Consumidor.Models;

namespace ClinicaMedica.Consumidor.Controllers
{
    public class MedicosController : Controller
    {
        private readonly ApiService _apiService;

        public MedicosController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var medicos = await _apiService.GetAllAsync<Medico>("api/medicosapi");
                return View(medicos);
            }
            catch (Exception ex)
            {
                ViewData["ErroApi"] = ex.Message;
                return View(new List<Medico>());
            }
        }
    }
}