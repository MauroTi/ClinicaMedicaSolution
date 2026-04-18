using ClinicaMedica.Web.ViewModels.Dashboard;

namespace ClinicaMedica.Web.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> ObterResumoAsync();
    }
}
