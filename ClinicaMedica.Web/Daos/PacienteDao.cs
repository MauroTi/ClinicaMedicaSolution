using ClinicaMedica.Web.Data;
using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.Models;
using Dapper;

namespace ClinicaMedica.Web.Daos
{
    public class PacienteDao : IPacienteDao
    {
        private readonly DbConnectionFactory _dbFactory;

        public PacienteDao(DbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public IEnumerable<Paciente> ObterTodos()
        {
            using var connection = _dbFactory.CreateConnection();
            const string sql = "SELECT * FROM pacientes";
            return connection.Query<Paciente>(sql);
        }

        public Paciente? ObterPorId(int id)
        {
            using var connection = _dbFactory.CreateConnection();
            const string sql = "SELECT * FROM pacientes WHERE Id = @Id";
            return connection.QueryFirstOrDefault<Paciente>(sql, new { Id = id });
        }

        public async Task<bool> ExisteCpfAsync(string cpf)
        {
            using var connection = _dbFactory.CreateConnection();
            const string sql = "SELECT COUNT(1) FROM pacientes WHERE Cpf = @Cpf";
            int count = await connection.ExecuteScalarAsync<int>(sql, new { Cpf = cpf });
            return count > 0;
        }

        public async Task CriarAsync(Paciente model)
        {
            using var connection = _dbFactory.CreateConnection();
            const string sql = @"
                INSERT INTO pacientes (Nome, Cpf, Telefone, Email, DataNascimento, Ativo, DataCadastro)
                VALUES (@Nome, @Cpf, @Telefone, @Email, @DataNascimento, @Ativo, @DataCadastro)";

            await connection.ExecuteAsync(sql, new
            {
                model.Nome,
                model.Cpf,
                model.Telefone,
                model.Email,
                model.DataNascimento,
                model.Ativo,
                model.DataCadastro
            });
        }

        public async Task<bool> EditarAsync(Paciente model)
        {
            using var connection = _dbFactory.CreateConnection();
            const string sql = @"
                UPDATE pacientes
                SET Nome = @Nome,
                    Cpf = @Cpf,
                    Telefone = @Telefone,
                    Email = @Email,
                    DataNascimento = @DataNascimento,
                    Ativo = @Ativo
                WHERE Id = @Id";

            var affectedRows = await connection.ExecuteAsync(sql, new
            {
                model.Nome,
                model.Cpf,
                model.Telefone,
                model.Email,
                model.DataNascimento,
                model.Ativo,
                model.Id
            });

            return affectedRows > 0;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            using var connection = _dbFactory.CreateConnection();
            const string sql = "DELETE FROM pacientes WHERE Id = @Id";
            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }
    }
}
