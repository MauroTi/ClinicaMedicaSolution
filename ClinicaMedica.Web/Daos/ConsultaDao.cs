using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.Data;
using ClinicaMedica.Web.DTOs;
using ClinicaMedica.Web.Models;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ClinicaMedica.Web.Daos
{
    public class ConsultaDao(DbConnectionFactory dbFactory) : IConsultaDao
    {
        private readonly DbConnectionFactory _dbFactory = dbFactory;

        #region QUERIES BASE

        private const string SelectBase = "SELECT * FROM consultas";

        private const string SelectDetalhado = @"
            SELECT c.Id, c.MedicoId, c.PacienteId, c.DataHoraConsulta, c.Valor, c.Status, c.Observacoes, c.DataCadastro,
                   m.Nome AS MedicoNome, m.Especialidade, m.Crm,
                   p.Nome AS PacienteNome, p.Telefone, p.Email
            FROM consultas c
            INNER JOIN medicos m ON c.MedicoId = m.Id
            INNER JOIN pacientes p ON c.PacienteId = p.Id";

        private const string InsertSql = @"
            INSERT INTO consultas (MedicoId, PacienteId, DataHoraConsulta, Valor, Status, Observacoes, DataCadastro)
            VALUES (@MedicoId, @PacienteId, @DataHoraConsulta, @Valor, @Status, @Observacoes, @DataCadastro)";

        #endregion

        #region SELECTS

        public async Task<IEnumerable<Consulta>> ObterTodosAsync()
        {
            using var conn = _dbFactory.CreateOpenConnection();
            return await conn.QueryAsync<Consulta>(SelectBase);
        }

        public async Task<Consulta?> ObterPorIdAsync(int id)
        {
            using var conn = _dbFactory.CreateOpenConnection();
            return await conn.QueryFirstOrDefaultAsync<Consulta>(
                $"{SelectBase} WHERE Id = @Id", new { Id = id });
        }

        public async Task<IEnumerable<ConsultaDto>> ObterTodosDetalhadosAsync()
        {
            using var conn = _dbFactory.CreateOpenConnection();
            return await conn.QueryAsync<ConsultaDto>(SelectDetalhado);
        }

        #endregion

        #region INSERT

        public async Task<int> InserirAsync(Consulta consulta)
        {
            if (_dbFactory.IsOracle)
                return await InserirOracleAsync(consulta);

            using var conn = _dbFactory.CreateOpenConnection();

            var sql = InsertSql + "; SELECT LAST_INSERT_ID();";

            return await conn.ExecuteScalarAsync<int>(sql, consulta);
        }

        private async Task<int> InserirOracleAsync(Consulta consulta)
        {
            using var conn = (OracleConnection)_dbFactory.CreateOpenConnection();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = InsertSql + " RETURNING Id INTO :Id";

            AddOracleParameters(cmd, consulta);

            var idParam = new OracleParameter("Id", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(idParam);

            await cmd.ExecuteNonQueryAsync();

            return ConvertOracleId(idParam.Value);
        }

        #endregion

        #region UPDATE / DELETE

        public async Task<bool> AtualizarAsync(Consulta consulta)
        {
            using var conn = _dbFactory.CreateOpenConnection();

            const string sql = @"
                UPDATE consultas
                SET MedicoId = @MedicoId,
                    PacienteId = @PacienteId,
                    DataHoraConsulta = @DataHoraConsulta,
                    Valor = @Valor,
                    Status = @Status,
                    Observacoes = @Observacoes
                WHERE Id = @Id";

            return (await conn.ExecuteAsync(sql, consulta)) > 0;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            using var conn = _dbFactory.CreateOpenConnection();

            const string sql = "DELETE FROM consultas WHERE Id = @Id";

            return (await conn.ExecuteAsync(sql, new { Id = id })) > 0;
        }

        #endregion

        #region HELPERS (ORACLE)

        private static void AddOracleParameters(OracleCommand cmd, Consulta c)
        {
            cmd.Parameters.Add(new OracleParameter("MedicoId", c.MedicoId));
            cmd.Parameters.Add(new OracleParameter("PacienteId", c.PacienteId));
            cmd.Parameters.Add(new OracleParameter("DataHoraConsulta", c.DataHoraConsulta));
            cmd.Parameters.Add(new OracleParameter("Valor", c.Valor));
            cmd.Parameters.Add(new OracleParameter("Status", c.Status));
            cmd.Parameters.Add(new OracleParameter("Observacoes", c.Observacoes ?? (object)DBNull.Value));
            cmd.Parameters.Add(new OracleParameter("DataCadastro", c.DataCadastro));
        }

        private static int ConvertOracleId(object value)
        {
            return value switch
            {
                OracleDecimal od => od.ToInt32(),
                decimal d => Convert.ToInt32(d),
                int i => i,
                _ => throw new InvalidOperationException("Erro ao converter Id do Oracle.")
            };
        }

        #endregion

        #region LEGADO

        IEnumerable<Consulta> IConsultaDao.ObterTodos()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}