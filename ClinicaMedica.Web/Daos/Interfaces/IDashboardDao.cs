using System.Threading.Tasks;

namespace ClinicaMedica.Web.Daos.Interfaces
{
    public interface IDashboardDao
    {
        Task<int> ObterTotalMedicosAsync();
        Task<int> ObterTotalPacientesAsync();
        Task<int> ObterTotalConsultasAsync();
        Task<decimal> ObterReceitaTotalAsync();
        Task<int> ConsultasPorStatusAsync(string status);
    }
}