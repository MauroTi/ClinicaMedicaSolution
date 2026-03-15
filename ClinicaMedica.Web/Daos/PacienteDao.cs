using ClinicaMedica.Web.Data;
using ClinicaMedica.Web.Daos;
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

        public IEnumerable<Paciente> ObterTodos()
        {
            string sql = "SELECT * FROM pacientes";
            return _dbConnection.Query<Paciente>(sql);
        }

        public Paciente? ObterPorId(int id)
        {
            string sql = "SELECT * FROM pacientes WHERE Id = @Id";
            return _dbConnection.QueryFirstOrDefault<Paciente>(sql, new { Id = id });
        }

        public async Task CriarAsync(Paciente model)
        {
            string sql = @"
                INSERT INTO pacientes (Nome, Telefone, Email, DataCadastro)
                VALUES (@Nome, @Telefone, @Email, @DataCadastro)";
            await _dbConnection.ExecuteAsync(sql, model);
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            string sql = "DELETE FROM pacientes WHERE Id = @Id";
            var affectedRows = await _dbConnection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }

        public async Task<bool> EditarAsync(Paciente model)
        {
            string sql = @"
                UPDATE pacientes 
                SET Nome=@Nome, Telefone=@Telefone, Email=@Email 
                WHERE Id=@Id";
            var affectedRows = await _dbConnection.ExecuteAsync(sql, model);
            return affectedRows > 0;
        }
    }
}