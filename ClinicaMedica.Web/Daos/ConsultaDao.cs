using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.Data;
using ClinicaMedica.Web.Interfaces;
using ClinicaMedica.Web.Models;
using ClinicaMedica.Web.Models.DTOs;
using Dapper;
using System.Data;

namespace ClinicaMedica.Web.Daos
{
    public class ConsultaDao : IConsultaDao
    {
        private readonly IDbConnection _dbConnection;

        public ConsultaDao(DbConnectionFactory dbFactory)
        {
            _dbConnection = dbFactory.CreateConnection();
        }

        public IEnumerable<Consulta> ObterTodos()
        {
            string sql = "SELECT * FROM consultas";
            return _dbConnection.Query<Consulta>(sql);
        }

        public Consulta? ObterPorId(int id)
        {
            string sql = "SELECT * FROM consultas WHERE Id = @Id";
            return _dbConnection.QueryFirstOrDefault<Consulta>(sql, new { Id = id });
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            string sql = "DELETE FROM consultas WHERE Id = @Id";
            var affectedRows = await _dbConnection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }

        public async Task CriarAsync(Consulta model)
        {
            string sql = @"
                INSERT INTO consultas (MedicoId, PacienteId, DataHoraConsulta, Valor, Status, Observacoes, DataCadastro)
                VALUES (@MedicoId, @PacienteId, @DataHoraConsulta, @Valor, @Status, @Observacoes, @DataCadastro)";
            await _dbConnection.ExecuteAsync(sql, model);
        }

        public Task<IEnumerable<Consulta>> ObterTodosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ConsultaDto>> ObterTodosDetalhadosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Consulta?> ObterPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> InserirAsync(Consulta consulta)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AtualizarAsync(Consulta consulta)
        {
            throw new NotImplementedException();
        }
    }
}