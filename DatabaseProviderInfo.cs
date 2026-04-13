using ClinicaMedica.Web.Infrastructure.Database.Providers;
using ClinicaMedica.Web.Models;
using System.Text.Json;

namespace ClinicaMedica.Web.Services
{
    public class DatabaseProviderInfo
    {
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public string Icon { get; set; } = string.Empty;
    }

    public class DatabaseConfigurationService : IDatabaseConfigurationService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DatabaseConfigurationService> _logger;
        private readonly string _configPath;

        public DatabaseConfigurationService(
            IConfiguration configuration,
            ILogger<DatabaseConfigurationService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _configPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        }

        public Task<string> GetCurrentProviderAsync()
        {
            try
            {
                var provider = _configuration["DatabaseSettings:Provider"] ?? "MySql";
                _logger.LogInformation($"Provider atual: {provider}");
                return Task.FromResult(provider);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao obter provider: {ex.Message}");
                return Task.FromResult("MySql");
            }
        }

        public async Task<bool> SwitchProviderAsync(string provider)
        {
            try
            {
                if (!IsValidProvider(provider))
                    throw new ArgumentException($"Provider '{provider}' não é válido");

                if (!File.Exists(_configPath))
                    throw new FileNotFoundException($"Arquivo de configuração não encontrado: {_configPath}");

                var json = await File.ReadAllTextAsync(_configPath);
                using var document = JsonDocument.Parse(json);
                var root = document.RootElement;

                // Criar novo objeto de configuração
                var newConfig = new Dictionary<string, object?>
                {
                    { "ConnectionStrings", root.GetProperty("ConnectionStrings").Deserialize<Dictionary<string, string>>() },
                    { "Logging", root.GetProperty("Logging").Deserialize<object>() },
                    { "DatabaseSettings", new Dictionary<string, string>
                        {
                            { "Provider", provider },
                            { "ConnectionStringName", provider == "Oracle" ? "OracleConnection" : "DefaultConnection" }
                        }
                    },
                    { "AllowedHosts", root.GetProperty("AllowedHosts").GetString() }
                };

                var options = new JsonSerializerOptions 
                { 
                    WriteIndented = true,
                    PropertyNamingPolicy = null
                };

                var updatedJson = JsonSerializer.Serialize(newConfig, options);
                await File.WriteAllTextAsync(_configPath, updatedJson);

                _logger.LogInformation($"✅ Provider alterado para: {provider}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"❌ Erro ao alternar provider: {ex.Message}");
                throw;
            }
        }

        public Task<List<DatabaseProviderInfo>> GetAvailableProvidersAsync()
        {
            var providers = new List<DatabaseProviderInfo>
            {
                new DatabaseProviderInfo
                {
                    Name = "MySql",
                    DisplayName = "MySQL",
                    Description = "Banco de dados relacional MySQL",
                    IsAvailable = true,
                    Icon = "fab fa-database"
                },
                new DatabaseProviderInfo
                {
                    Name = "Oracle",
                    DisplayName = "Oracle Database",
                    Description = "Banco de dados empresarial Oracle",
                    IsAvailable = true,
                    Icon = "fab fa-database"
                }
            };

            return Task.FromResult(providers);
        }

        public async Task<DatabaseConnectionInfo> GetConnectionInfoAsync()
        {
            try
            {
                var provider = await GetCurrentProviderAsync();
                var connectionStringName = provider == "Oracle" ? "OracleConnection" : "DefaultConnection";
                var connectionString = _configuration.GetConnectionString(connectionStringName);

                return new DatabaseConnectionInfo
                {
                    Provider = provider,
                    ConnectionStringName = connectionStringName,
                    ConnectionString = MaskConnectionString(connectionString),
                    IsConnected = await TestConnectionAsync(provider)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao obter info de conexão: {ex.Message}");
                return new DatabaseConnectionInfo
                {
                    Provider = "Desconhecido",
                    ConnectionStringName = "Erro",
                    ConnectionString = "Erro ao carregar",
                    IsConnected = false
                };
            }
        }

        private async Task<bool> TestConnectionAsync(string provider)
        {
            try
            {
                var connectionStringName = provider == "Oracle" ? "OracleConnection" : "DefaultConnection";
                var connectionString = _configuration.GetConnectionString(connectionStringName);

                if (string.IsNullOrEmpty(connectionString))
                    return false;

                if (provider == "Oracle")
                {
                    using var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(connectionString);
                    await connection.OpenAsync();
                    connection.Close();
                }
                else
                {
                    using var connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
                    await connection.OpenAsync();
                    connection.Close();
                }

                _logger.LogInformation($"✅ Conexão com {provider} testada com sucesso");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"❌ Erro ao testar conexão com {provider}: {ex.Message}");
                return false;
            }
        }

        private bool IsValidProvider(string provider)
        {
            return provider == "MySql" || provider == "Oracle";
        }

        private string MaskConnectionString(string? connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                return string.Empty;

            return System.Text.RegularExpressions.Regex.Replace(
                connectionString,
                @"(Password|password|pwd)=([^;]*)",
                "$1=***",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }
    }
}