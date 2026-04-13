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

            string sql = @"
                INSERT INTO MEDICOS 
                (ID, NOME, CRM, ESPECIALIDADE, TELEFONE, EMAIL, ATIVO, DATACADASTRO)
                VALUES 
                (SEQ_MEDICOS.NEXTVAL, @Nome, @Crm, @Especialidade, @Telefone, @Email, @Ativo, @DataCadastro)";

            int rows = await connection.ExecuteAsync(sql, medico);
            return rows > 0;
        }

        public async Task<bool> AtualizarAsync(Medico medico)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            string sql = @"
                UPDATE MEDICOS
                SET NOME = @Nome,
                    CRM = @Crm,
                    ESPECIALIDADE = @Especialidade,
                    TELEFONE = @Telefone,
                    EMAIL = @Email,
                    ATIVO = @Ativo
                WHERE ID = @Id";

            int rows = await connection.ExecuteAsync(sql, medico);
            return rows > 0;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            string sql = @"DELETE FROM MEDICOS WHERE ID = @Id";

            int rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }

        public async Task<Medico> ObterPorId(int id)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            string sql = @"SELECT * FROM MEDICOS WHERE ID = @Id";

            return await connection.QueryFirstOrDefaultAsync<Medico>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Medico>> ObterTodos()
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            string sql = @"SELECT * FROM MEDICOS";

            return await connection.QueryAsync<Medico>(sql);
        }

        public async Task<Medico> ObterPorCrmAsync(string crm)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            string sql = @"SELECT * FROM MEDICOS WHERE CRM = @Crm";

            return await connection.QueryFirstOrDefaultAsync<Medico>(sql, new { Crm = crm });
        }
    }
}