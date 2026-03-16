// Path: Daos/MedicoDao.cs
using ClinicaMedica.Web.Data;
using ClinicaMedica.Web.Models;
using ClinicaMedica.Web.Daos.Interfaces;
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

        public async Task<bool> AdicionarAsync(Medico medico)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            string sql = @"INSERT INTO medicos (Nome, Crm, Especialidade, Telefone, Email, Ativo, DataCadastro) 
                           VALUES (@Nome, @Crm, @Especialidade, @Telefone, @Email, @Ativo, @DataCadastro)";
            int rows = await connection.ExecuteAsync(sql, medico);
            return rows > 0;
        }

        public async Task<bool> AtualizarAsync(Medico medico)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            string sql = @"UPDATE medicos
                           SET Nome = @Nome, Crm = @Crm, Especialidade = @Especialidade,
                               Telefone = @Telefone, Email = @Email, Ativo = @Ativo
                           WHERE Id = @Id";
            int rows = await connection.ExecuteAsync(sql, medico);
            return rows > 0;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            string sql = @"DELETE FROM medicos WHERE Id = @Id";
            int rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }

        public async Task<Medico> ObterPorId(int id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            string sql = @"SELECT * FROM medicos WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Medico>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Medico>> ObterTodos()
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            string sql = @"SELECT * FROM medicos";
            return await connection.QueryAsync<Medico>(sql);
        }

        public async Task<Medico> ObterPorCrmAsync(string crm)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            string sql = @"SELECT * FROM medicos WHERE Crm = @Crm";
            return await connection.QueryFirstOrDefaultAsync<Medico>(sql, new { Crm = crm });
        }
    }
}