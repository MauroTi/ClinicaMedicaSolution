namespace ClinicaMedica.Web.Services
{
    public interface IDatabaseConfigurationService
    {
        Task<string> GetCurrentProviderAsync();
        Task<bool> SwitchProviderAsync(string provider);
        Task<List<DatabaseProviderInfo>> GetAvailableProvidersAsync();
        Task<DatabaseConnectionInfo> GetConnectionInfoAsync();
    }

    public class DatabaseProviderInfo
    {
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public string Icon { get; set; } = string.Empty;
    }

    public class DatabaseConnectionInfo
    {
        public string Provider { get; set; } = string.Empty;
        public string ConnectionStringName { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public bool IsConnected { get; set; }
    }
}