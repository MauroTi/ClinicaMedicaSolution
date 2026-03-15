using ClinicaMedica.Web.Data;
using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.Models;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                SELECT Id, Nome, Especialidade, Crm, Telefone, Email, Ativo, DataCadastro
                FROM medicos
                ORDER BY Nome";

            return connection.Query<Medico>(sql);
        }

        public Medico? ObterPorId(int id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            const string sql = @"
                SELECT Id, Nome, Especialidade, Crm, Telefone, Email, Ativo, DataCadastro
                FROM medicos
                WHERE Id = @Id";

            return connection.QueryFirstOrDefault<Medico>(sql, new { Id = id });
        }

        public async Task AdicionarAsync(Medico medico)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            const string sql = @"
                INSERT INTO medicos (Nome, Especialidade, Crm, Telefone, Email, Ativo, DataCadastro)
                VALUES (@Nome, @Especialidade, @Crm, @Telefone, @Email, @Ativo, @DataCadastro);";

            await connection.ExecuteAsync(sql, medico);
        }

        public async Task<bool> AtualizarAsync(Medico medico)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            const string sql = @"
                UPDATE medicos
                SET Nome = @Nome,
                    Especialidade = @Especialidade,
                    Crm = @Crm,
                    Telefone = @Telefone,
                    Email = @Email,
                    Ativo = @Ativo
                WHERE Id = @Id;";

            var rows = await connection.ExecuteAsync(sql, medico);
            return rows > 0;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            const string sql = @"DELETE FROM medicos WHERE Id = @Id;";

            var rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}