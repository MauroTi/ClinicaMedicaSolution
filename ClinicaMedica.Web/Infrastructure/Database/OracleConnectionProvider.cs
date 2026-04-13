using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ClinicaMedica.Web.Infrastructure.Database.Providers
{
    public class OracleConnectionProvider : IDbConnectionProvider
    {
        public DatabaseProvider Provider => DatabaseProvider.Oracle;

        public IDbConnection CreateConnection(string connectionString)
            => new OracleConnection(connectionString);

        public string GetConnectionStringName() => "OracleConnection";
    }
}