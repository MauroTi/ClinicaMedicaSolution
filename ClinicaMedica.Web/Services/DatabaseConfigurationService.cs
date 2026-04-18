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
        private readonly IConfigurationRoot _configRoot;
        private readonly string _configPath;

        public DatabaseConfigurationService(
            IConfiguration configuration,
            ILogger<DatabaseConfigurationService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _configRoot = (IConfigurationRoot)configuration;
            _configPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        }

        public Task<string> GetCurrentProviderAsync()
        {
            try
            {
                var provider = NormalizeProvider(_configuration["DatabaseSettings:Provider"]);
                _logger.LogInformation("Provider atual: {Provider}", provider);
                return Task.FromResult(provider);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter provider.");
                return Task.FromResult(DatabaseProvider.MySql.ToString());
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

                var normalizedProvider = NormalizeProvider(provider);
                var json = await File.ReadAllTextAsync(_configPath);
                using var document = JsonDocument.Parse(json);
                var root = document.RootElement;

                var newConfig = new Dictionary<string, object?>
                {
                    { "ConnectionStrings", root.GetProperty("ConnectionStrings").Deserialize<Dictionary<string, string>>() },
                    { "Logging", root.GetProperty("Logging").Deserialize<object>() },
                    { "DatabaseSettings", new Dictionary<string, string>
                        {
                            { "Provider", normalizedProvider },
                            { "ConnectionStringName", GetConnectionStringName(normalizedProvider) }
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
                _configRoot.Reload();

                _logger.LogInformation("Provider alterado para: {Provider}", normalizedProvider);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao alternar provider.");
                throw;
            }
        }

        public Task<List<DatabaseProviderInfo>> GetAvailableProvidersAsync()
        {
            var providers = new List<DatabaseProviderInfo>
            {
                new()
                {
                    Name = "MySql",
                    DisplayName = "MySQL",
                    Description = "Banco de dados relacional MySQL",
                    IsAvailable = true,
                    Icon = "fab fa-database"
                },
                new()
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
                var connectionStringName = GetConnectionStringName(provider);
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
                _logger.LogError(ex, "Erro ao obter info de conexão.");
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
                var normalizedProvider = NormalizeProvider(provider);
                var connectionStringName = GetConnectionStringName(normalizedProvider);
                var connectionString = _configuration.GetConnectionString(connectionStringName);

                if (string.IsNullOrWhiteSpace(connectionString))
                    return false;

                if (normalizedProvider == DatabaseProvider.Oracle.ToString())
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

                _logger.LogInformation("Conexão com {Provider} testada com sucesso.", normalizedProvider);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao testar conexão com {Provider}.", provider);
                return false;
            }
        }

        private static bool IsValidProvider(string provider)
        {
            return Enum.TryParse<DatabaseProvider>(provider, true, out _);
        }

        private static string NormalizeProvider(string? provider)
        {
            return Enum.TryParse<DatabaseProvider>(provider, true, out var parsed)
                ? parsed.ToString()
                : DatabaseProvider.MySql.ToString();
        }

        private static string GetConnectionStringName(string provider)
        {
            return Enum.TryParse<DatabaseProvider>(provider, true, out var parsed) && parsed == DatabaseProvider.Oracle
                ? "OracleConnection"
                : "MySqlConnection";
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
