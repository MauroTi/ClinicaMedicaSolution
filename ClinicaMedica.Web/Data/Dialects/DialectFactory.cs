using Microsoft.Extensions.Configuration;

namespace ClinicaMedica.Web.Data.Dialects
{
    public class DialectFactory
    {
        private readonly IConfiguration _config;

        public DialectFactory(IConfiguration config)
        {
            _config = config;
        }

        public ISqlDialect Criar()
        {
            var provider = _config["DatabaseProvider"];

            if (provider == "MySql")
                return new MySqlDialect();

            if (provider == "Oracle")
                return new OracleDialect();

            throw new Exception("Dialeto não suportado");
        }
    }
}