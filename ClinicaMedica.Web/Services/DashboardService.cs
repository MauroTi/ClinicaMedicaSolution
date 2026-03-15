// Path: Services/DashboardService.cs
using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.ViewModels.Dashboard;

public class DashboardService
{
    private readonly IDashboardDao _dashboardDao;

    public DashboardService(IDashboardDao dashboardDao)
    {
        _dashboardDao = dashboardDao;
    }

    public DashboardResumoViewModel ObterDadosResumo()
    {
        return new DashboardResumoViewModel
        {
            TotalMedicos = _dashboardDao.ObterTotalMedicos(),
            TotalPacientes = _dashboardDao.ObterTotalPacientes(),
            TotalConsultas = _dashboardDao.ObterTotalConsultas(),
            ReceitaTotal = _dashboardDao.ObterReceitaTotal()
        };
    }
}