using System.Data;
using ClinicaMedica.Web.Infrastructure.Database.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ClinicaMedica.Web.Data
{
    public class DbConnectionFactory
    {
        private readonly IEnumerable<IDbConnectionProvider> _providers;
        private readonly IDatabaseProviderResolver _resolver;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DbConnectionFactory> _logger;
        private DatabaseProvider? _cachedProvider;

        public DbConnectionFactory(
            IEnumerable<IDbConnectionProvider> providers,
            IDatabaseProviderResolver resolver,
            IConfiguration configuration,
            ILogger<DbConnectionFactory> logger)
        {
            _providers = providers;
            _resolver = resolver;
            _configuration = configuration;
            _logger = logger;
        }

        // 🔥 COMPATIBILIDADE (mantido)
        public bool IsOracle => GetCurrentProvider() == DatabaseProvider.Oracle;
        public bool IsMySql => GetCurrentProvider() == DatabaseProvider.MySql;

        private DatabaseProvider GetCurrentProvider()
        {
            if (_cachedProvider.HasValue)
                return _cachedProvider.Value;

            _cachedProvider = _resolver.GetProvider();

            _logger.LogInformation($"Provider atual: {_cachedProvider.Value}");

            return _cachedProvider.Value;
        }

        private IDbConnectionProvider GetProvider()
        {
            var providerType = GetCurrentProvider();

            var provider = _providers.FirstOrDefault(p => p.Provider == providerType);

            if (provider == null)
                throw new NotSupportedException($"Provider '{providerType}' não registrado.");

            return provider;
        }

        public IDbConnection CreateConnection()
        {
            var provider = GetProvider();
            var connName = provider.GetConnectionStringName();

            var connectionString = _configuration.GetConnectionString(connName);

            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException($"Connection string '{connName}' não encontrada.");

            _logger.LogInformation($"Criando conexão com {provider.Provider}");

            return provider.CreateConnection(connectionString);
        }

        // 🔥 RESTAURADO (resolve seus erros)
        public IDbConnection CreateOpenConnection()
        {
            var connection = CreateConnection();

            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao conectar com {_resolver.GetProvider()}");

                throw new Exception("Banco selecionado indisponível. Verifique conexão.", ex);
            }

            return connection;
        }
    }
}