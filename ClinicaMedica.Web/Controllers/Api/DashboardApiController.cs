using ClinicaMedica.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedica.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardApiController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardApiController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // GET: api/Dashboard
        [HttpGet]
        public IActionResult Get()
        {
            var resumo = _dashboardService.ObterResumo();
            return Ok(resumo);
        }
    }
}