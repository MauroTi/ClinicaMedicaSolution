using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.Data;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace ClinicaMedica.Web.Daos
{
    public class DashboardDao : IDashboardDao
    {
        private readonly DbConnectionFactory _dbConnectionFactory;

        public DashboardDao(DbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<int> ObterTotalMedicosAsync()
        {
            using IDbConnection db = _dbConnectionFactory.CreateConnection();
            return await db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM medicos");
        }

        public async Task<int> ObterTotalPacientesAsync()
        {
            using IDbConnection db = _dbConnectionFactory.CreateConnection();
            return await db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM pacientes");
        }

        public async Task<int> ObterTotalConsultasAsync()
        {
            using IDbConnection db = _dbConnectionFactory.CreateConnection();
            return await db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM consultas");
        }

        public async Task<decimal> ObterReceitaTotalAsync()
        {
            using IDbConnection db = _dbConnectionFactory.CreateConnection();
            return await db.ExecuteScalarAsync<decimal>("SELECT IFNULL(SUM(Valor),0) FROM consultas WHERE Status='Realizada'");
        }

        public async Task<int> ConsultasPorStatusAsync(string status)
        {
            using IDbConnection db = _dbConnectionFactory.CreateConnection();
            return await db.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM consultas WHERE Status = @Status",
                new { Status = status }
            );
        }
    }
}