using ClinicaMedica.Web.Models;

namespace ClinicaMedica.Web.Services
{
    public interface IDatabaseConfigurationService
    {
        Task<string> GetCurrentProviderAsync();
        Task<bool> SwitchProviderAsync(string provider);
        Task<DatabaseConnectionInfo> GetConnectionInfoAsync();
    }
}