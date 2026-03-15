using ClinicaMedica.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedica.Web.Controllers
{
    public class ConsultasController : Controller
    {
        private readonly IConsultaService _consultaService;

        public ConsultasController(IConsultaService consultaService)
        {
            _consultaService = consultaService;
        }

        public async Task<IActionResult> Index()
        {
            var consultas = await _consultaService.ObterTodosDetalhadosAsync();
            return View(consultas);
        }
    }
}