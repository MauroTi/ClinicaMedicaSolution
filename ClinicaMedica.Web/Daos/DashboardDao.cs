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

        private bool IsOracle(IDbConnection db)
        {
            return db.GetType().Name.Contains("Oracle");
        }

        public async Task<int> ObterTotalMedicosAsync()
        {
            using IDbConnection db = _dbConnectionFactory.CreateConnection();

            var sql = IsOracle(db)
                ? "SELECT COUNT(*) FROM CLINICA.MEDICOS"
                : "SELECT COUNT(*) FROM medicos";

            return await db.ExecuteScalarAsync<int>(sql);
        }

        public async Task<int> ObterTotalPacientesAsync()
        {
            using IDbConnection db = _dbConnectionFactory.CreateConnection();

            var sql = IsOracle(db)
                ? "SELECT COUNT(*) FROM CLINICA.PACIENTES"
                : "SELECT COUNT(*) FROM pacientes";

            return await db.ExecuteScalarAsync<int>(sql);
        }

        public async Task<int> ObterTotalConsultasAsync()
        {
            using IDbConnection db = _dbConnectionFactory.CreateConnection();

            var sql = IsOracle(db)
                ? "SELECT COUNT(*) FROM CLINICA.CONSULTAS"
                : "SELECT COUNT(*) FROM consultas";

            return await db.ExecuteScalarAsync<int>(sql);
        }

        public async Task<decimal> ObterReceitaTotalAsync()
        {
            using IDbConnection db = _dbConnectionFactory.CreateConnection();

            var sql = IsOracle(db)
                ? @"SELECT NVL(SUM(VALOR),0) 
                    FROM CLINICA.CONSULTAS 
                    WHERE STATUS = 'REALIZADA'"
                : @"SELECT COALESCE(SUM(Valor),0) 
                    FROM consultas 
                    WHERE Status = 'Realizada'";

            return await db.ExecuteScalarAsync<decimal>(sql);
        }

        public async Task<int> ConsultasPorStatusAsync(string status)
        {
            using IDbConnection db = _dbConnectionFactory.CreateConnection();

            var statusNormalizado = status.ToUpper();

            if (IsOracle(db))
            {
                return await db.ExecuteScalarAsync<int>(
                    @"SELECT COUNT(*) 
              FROM CLINICA.CONSULTAS 
              WHERE STATUS = :status",
                    new { status = statusNormalizado }
                );
            }
            else
            {
                return await db.ExecuteScalarAsync<int>(
                    @"SELECT COUNT(*) 
              FROM consultas 
              WHERE UPPER(Status) = @status",
                    new { status = statusNormalizado }
                );
            }
        }
    }
    }
