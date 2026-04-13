using MySql.Data.MySqlClient;
using System.Data;

namespace ClinicaMedica.Web.Infrastructure.Database.Providers
{
    public class MySqlConnectionProvider : IDbConnectionProvider
    {
        public DatabaseProvider Provider => DatabaseProvider.MySql;

        public IDbConnection CreateConnection(string connectionString)
            => new MySqlConnection(connectionString);

        public string GetConnectionStringName() => "MySqlConnection";
    }
}