namespace ClinicaMedica.Web.Services
{
    public class DatabaseConnectionInfo
    {
        public string Provider { get; set; } = string.Empty;
        public string ConnectionStringName { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public bool IsConnected { get; set; }
    }
}