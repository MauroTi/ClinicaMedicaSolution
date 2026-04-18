using ClinicaMedica.Web.Configuration;
using ClinicaMedica.Web.Infrastructure.Database.Providers;
using Microsoft.Extensions.Options;

namespace ClinicaMedica.Web.Data.Dialects
{
    public class DialectFactory
    {
        private readonly DatabaseSettings _settings;
        private readonly IDatabaseProviderResolver _providerResolver;

        public DialectFactory(
            IOptions<DatabaseSettings> settings,
            IDatabaseProviderResolver providerResolver)
        {
            _settings = settings.Value;
            _providerResolver = providerResolver;
        }

        public ISqlDialect Criar()
        {
            var provider = ResolveProvider();

            return provider switch
            {
                DatabaseProvider.MySql => new MySqlDialect(),
                DatabaseProvider.Oracle => new OracleDialect(),
                _ => throw new Exception($"Dialeto não suportado: {provider}")
            };
        }

        private DatabaseProvider ResolveProvider()
        {
            if (Enum.TryParse<DatabaseProvider>(_settings.Provider, true, out var configuredProvider))
            {
                try
                {
                    return _providerResolver.GetProvider();
                }
                catch
                {
                    return configuredProvider;
                }
            }

            return _providerResolver.GetProvider();
        }
    }
}
