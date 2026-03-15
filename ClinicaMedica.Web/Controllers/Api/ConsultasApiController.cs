using ClinicaMedica.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedica.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultasController : ControllerBase
    {
        private readonly IConsultaService _consultaService;

        public ConsultasController(IConsultaService consultaService)
        {
            _consultaService = consultaService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var consultas = await _consultaService.ObterTodosDetalhadosAsync();
            return Ok(consultas);
        }
    }
}