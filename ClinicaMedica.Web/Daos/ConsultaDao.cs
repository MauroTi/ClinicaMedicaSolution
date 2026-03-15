using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.Data;
using ClinicaMedica.Web.Interfaces;
using ClinicaMedica.Web.Models;
using ClinicaMedica.Web.Models.DTOs;
using Dapper;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicaMedica.Web.Daos
{
    public class ConsultaDao : IConsultaDao
    {
        private readonly IDbConnection _dbConnection;

        public ConsultaDao(DbConnectionFactory dbFactory)
        {
            _dbConnection = dbFactory.CreateConnection();
        }

        // Obter todas as consultas (simples)
        public async Task<IEnumerable<Consulta>> ObterTodosAsync()
        {
            string sql = "SELECT * FROM consultas";
            return await _dbConnection.QueryAsync<Consulta>(sql);
        }

        // Obter consulta por ID (assíncrono)
        public async Task<Consulta?> ObterPorIdAsync(int id)
        {
            string sql = "SELECT * FROM consultas WHERE Id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<Consulta>(sql, new { Id = id });
        }

        // Obter todas as consultas com detalhes (DTO)
        public async Task<IEnumerable<ConsultaDto>> ObterTodosDetalhadosAsync()
        {
            string sql = @"
                SELECT c.Id, c.MedicoId, c.PacienteId, c.DataHoraConsulta, c.Valor, c.Status, c.Observacoes, c.DataCadastro,
                       m.Nome AS MedicoNome, m.Especialidade, m.Crm,
                       p.Nome AS PacienteNome, p.Telefone, p.Email
                FROM consultas c
                INNER JOIN medicos m ON c.MedicoId = m.Id
                INNER JOIN pacientes p ON c.PacienteId = p.Id
            ";

            var consultas = await _dbConnection.QueryAsync<ConsultaDto>(sql);
            return consultas;
        }

        // Criar nova consulta
        public async Task CriarAsync(Consulta model)
        {
            string sql = @"
                INSERT INTO consultas (MedicoId, PacienteId, DataHoraConsulta, Valor, Status, Observacoes, DataCadastro)
                VALUES (@MedicoId, @PacienteId, @DataHoraConsulta, @Valor, @Status, @Observacoes, @DataCadastro)";
            await _dbConnection.ExecuteAsync(sql, model);
        }

        // Excluir consulta
        public async Task<bool> ExcluirAsync(int id)
        {
            string sql = "DELETE FROM consultas WHERE Id = @Id";
            var affectedRows = await _dbConnection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }

        // Atualizar consulta
        public async Task<bool> AtualizarAsync(Consulta consulta)
        {
            string sql = @"
                UPDATE consultas
                SET MedicoId = @MedicoId,
                    PacienteId = @PacienteId,
                    DataHoraConsulta = @DataHoraConsulta,
                    Valor = @Valor,
                    Status = @Status,
                    Observacoes = @Observacoes
                WHERE Id = @Id";
            var affectedRows = await _dbConnection.ExecuteAsync(sql, consulta);
            return affectedRows > 0;
        }

        // Inserir consulta e retornar ID
        public async Task<int> InserirAsync(Consulta consulta)
        {
            string sql = @"
                INSERT INTO consultas (MedicoId, PacienteId, DataHoraConsulta, Valor, Status, Observacoes, DataCadastro)
                VALUES (@MedicoId, @PacienteId, @DataHoraConsulta, @Valor, @Status, @Observacoes, @DataCadastro);
                SELECT LAST_INSERT_ID();";
            var id = await _dbConnection.ExecuteScalarAsync<int>(sql, consulta);
            return id;
        }

        IEnumerable<Consulta> IConsultaDao.ObterTodos()
        {
            throw new NotImplementedException();
        }
    }
}