// Path: Controllers/DashboardController.cs
using ClinicaMedica.Web.ViewModels.Dashboard;
using Microsoft.AspNetCore.Mvc;

public class DashboardController : Controller
{
    private readonly DashboardService _dashboardService;

    public DashboardController(DashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public IActionResult Index()
    {
        // Obtém os dados do serviço
        var resumo = _dashboardService.ObterDadosResumo();

        // Mapeia para o ViewModel específico da View
        var viewModel = new DashboardViewModel
        {
            TotalMedicos = resumo.TotalMedicos,
            TotalPacientes = resumo.TotalPacientes,
            TotalConsultas = resumo.TotalConsultas,
            ReceitaTotal = resumo.ReceitaTotal,
            ConsultasAgendadas = resumo.ConsultasAgendadas,
        };

        return View(viewModel);
    }
}