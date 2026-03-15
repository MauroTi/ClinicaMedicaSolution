using System.Data;
using MySql.Data.MySqlClient;
using ClinicaMedica.Web.Data.Interfaces;

namespace ClinicaMedica.Web.Data
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public ConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("A connection string 'DefaultConnection' não foi encontrada.");
            }

            return new MySqlConnection(connectionString);
        }
    }
}