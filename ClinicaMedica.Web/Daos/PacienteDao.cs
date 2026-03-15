using ClinicaMedica.Web.Data;
using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.Models;
using Dapper;
using System.Data;

namespace ClinicaMedica.Web.Daos
{
    public class PacienteDao : IPacienteDao
    {
        private readonly IDbConnection _dbConnection;

        public PacienteDao(DbConnectionFactory dbFactory)
        {
            _dbConnection = dbFactory.CreateConnection();
        }

        // Lista todos os pacientes
        public IEnumerable<Paciente> ObterTodos()
        {
            string sql = "SELECT * FROM pacientes";
            return _dbConnection.Query<Paciente>(sql);
        }

        // Busca paciente por ID
        public Paciente? ObterPorId(int id)
        {
            string sql = "SELECT * FROM pacientes WHERE Id = @Id";
            return _dbConnection.QueryFirstOrDefault<Paciente>(sql, new { Id = id });
        }

        // Cria novo paciente

        public async Task<bool> ExisteCpfAsync(string cpf)
        {
            string sql = "SELECT COUNT(1) FROM pacientes WHERE Cpf = @Cpf";
            int count = await _dbConnection.ExecuteScalarAsync<int>(sql, new { Cpf = cpf });
            return count > 0;
        }
        public async Task CriarAsync(Paciente model)
        {
            string sql = @"
                INSERT INTO pacientes (Nome, Cpf, Telefone, Email, DataNascimento, Ativo, DataCadastro)
                VALUES (@Nome, @Cpf, @Telefone, @Email, @DataNascimento, @Ativo, @DataCadastro)";

            // Usando objeto anônimo para garantir mapeamento correto
            await _dbConnection.ExecuteAsync(sql, new
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

        // Atualiza paciente existente
        public async Task<bool> EditarAsync(Paciente model)
        {
            string sql = @"
                UPDATE pacientes
                SET Nome = @Nome,
                    Cpf = @Cpf,
                    Telefone = @Telefone,
                    Email = @Email,
                    DataNascimento = @DataNascimento,
                    Ativo = @Ativo
                WHERE Id = @Id";

            var affectedRows = await _dbConnection.ExecuteAsync(sql, new
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

        // Exclui paciente
        public async Task<bool> ExcluirAsync(int id)
        {
            string sql = "DELETE FROM pacientes WHERE Id = @Id";
            var affectedRows = await _dbConnection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }
    }
}