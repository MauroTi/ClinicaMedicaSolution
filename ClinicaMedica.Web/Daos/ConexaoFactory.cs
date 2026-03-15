using MySql.Data.MySqlClient;
using System.Data;

namespace ClinicaMedica.Web.Daos
{
    public class ConexaoFactory
    {
        private readonly IConfiguration _configuration;

        public ConexaoFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CriarConexao()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            return new MySqlConnection(connectionString);
        }
    }
}