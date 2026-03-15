using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.ViewModels.Dashboard;
using System.Threading.Tasks;

namespace ClinicaMedica.Web.Services
{
    public class DashboardService
    {
        private readonly IDashboardDao _dashboardDao;

        public DashboardService(IDashboardDao dashboardDao)
        {
            _dashboardDao = dashboardDao;
        }

        public async Task<DashboardViewModel> ObterDadosResumoAsync()
        {
            return new DashboardViewModel
            {
                TotalMedicos = await _dashboardDao.ObterTotalMedicosAsync(),
                TotalPacientes = await _dashboardDao.ObterTotalPacientesAsync(),
                TotalConsultas = await _dashboardDao.ObterTotalConsultasAsync(),
                ReceitaTotal = await _dashboardDao.ObterReceitaTotalAsync(),
                ConsultasAgendadas = await _dashboardDao.ConsultasPorStatusAsync("Agendada"),
                ConsultasRealizadas = await _dashboardDao.ConsultasPorStatusAsync("Realizada"),
                ConsultasCanceladas = await _dashboardDao.ConsultasPorStatusAsync("Cancelada"),
            };
        }
    }
}