using ClinicaMedica.Web.Data;
using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.Models;
using Dapper;

namespace ClinicaMedica.Web.Daos
{
    public class MedicoDao : IMedicoDao
    {
        private readonly DbConnectionFactory _dbConnectionFactory;

        public MedicoDao(DbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public IEnumerable<Medico> ObterTodos()
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            const string sql = @"
                SELECT 
                    Id,
                    Nome,
                    Especialidade,
                    Crm,
                    Telefone,
                    Email,
                    Ativo,
                    DataCadastro
                FROM medicos";

            return connection.Query<Medico>(sql);
        }

        public Medico? ObterPorId(int id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            const string sql = @"
                SELECT 
                    Id,
                    Nome,
                    Especialidade,
                    Crm,
                    Telefone,
                    Email,
                    Ativo,
                    DataCadastro
                FROM medicos
                WHERE Id = @Id";

            return connection.QueryFirstOrDefault<Medico>(sql, new { Id = id });
        }
    }
}