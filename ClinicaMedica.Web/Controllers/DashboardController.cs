// Path: Controllers/DashboardController.cs
using ClinicaMedica.Web.Services;
using ClinicaMedica.Web.ViewModels.Dashboard;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClinicaMedica.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DashboardService _dashboardService;

        public DashboardController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // Método assíncrono para obter dados do dashboard
        public async Task<IActionResult> Index()
        {
            // Obtém os dados do serviço de forma assíncrona
            var resumo = await _dashboardService.ObterDadosResumoAsync();

            // Mapeia diretamente para o ViewModel da View
            var viewModel = new DashboardViewModel
            {
                TotalMedicos = resumo.TotalMedicos,
                TotalPacientes = resumo.TotalPacientes,
                TotalConsultas = resumo.TotalConsultas,
                ReceitaTotal = resumo.ReceitaTotal,
                ConsultasAgendadas = resumo.ConsultasAgendadas,
                ConsultasRealizadas = resumo.ConsultasRealizadas,
                ConsultasCanceladas = resumo.ConsultasCanceladas
            };

            return View(viewModel);
        }
    }
}