using ClinicaMedica.Web.Core.Entities;
using ClinicaMedica.Web.Data;
using Dapper;

namespace ClinicaMedica.Web.Infrastructure.Repositories
{
    public class MedicoRepository : IGenericRepository<Medico>
    {
        private readonly DbConnectionFactory _dbFactory;
        private readonly ILogger<MedicoRepository> _logger;

        public MedicoRepository(DbConnectionFactory dbFactory, ILogger<MedicoRepository> logger)
        {
            _dbFactory = dbFactory;
            _logger = logger;
        }

        public async Task<Medico?> GetByIdAsync(int id)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                const string sql = "SELECT * FROM MEDICOS WHERE ID = @Id";
                return await conn.QueryFirstOrDefaultAsync<Medico>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao buscar médico: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<Medico>> GetAllAsync()
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                const string sql = "SELECT * FROM MEDICOS ORDER BY NOME";
                return await conn.QueryAsync<Medico>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao listar médicos: {ex.Message}");
                return [];
            }
        }

        public async Task<bool> AddAsync(Medico entity)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                const string sql = @"
                    INSERT INTO MEDICOS (NOME, CRM, ESPECIALIDADE, TELEFONE, EMAIL, ATIVO, DATACADASTRO)
                    VALUES (@Nome, @Crm, @Especialidade, @Telefone, @Email, @Ativo, @DataCadastro)";

                var result = await conn.ExecuteAsync(sql, entity);
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao adicionar médico: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Medico entity)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                const string sql = @"
                    UPDATE MEDICOS 
                    SET NOME = @Nome, CRM = @Crm, ESPECIALIDADE = @Especialidade, 
                        TELEFONE = @Telefone, EMAIL = @Email, ATIVO = @Ativo
                    WHERE ID = @Id";

                var result = await conn.ExecuteAsync(sql, entity);
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao atualizar médico: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                using var conn = _dbFactory.CreateConnection();
                const string sql = "DELETE FROM MEDICOS WHERE ID = @Id";
                var result = await conn.ExecuteAsync(sql, new { Id = id });
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao deletar médico: {ex.Message}");
                return false;
            }
        }
    }
}