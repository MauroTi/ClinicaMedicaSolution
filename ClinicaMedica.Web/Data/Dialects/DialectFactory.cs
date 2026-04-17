using ClinicaMedica.Web.Configuration;
using Microsoft.Extensions.Options;

namespace ClinicaMedica.Web.Data.Dialects
{
    public class DialectFactory
    {
        private readonly DatabaseSettings _settings;

        public DialectFactory(IOptions<DatabaseSettings> settings)
        {
            _settings = settings.Value;
        }

        public ISqlDialect Criar()
        {
            var provider = _settings.Provider?.Trim().ToLower();

            return provider switch
            {
                "mysql" => new MySqlDialect(),
                "oracle" => new OracleDialect(),
                _ => throw new Exception($"Dialeto não suportado: {provider}")
            };
        }
    }
}