using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.Data;
using ClinicaMedica.Web.Data.Dialects;
using ClinicaMedica.Web.DTOs;
using ClinicaMedica.Web.Models;
using Dapper;
using System.Data;

namespace ClinicaMedica.Web.Daos;

public class ConsultaDao : IConsultaDao
{
    private const string SelectColumns = @"
            SELECT 
                ID AS Id,
                MEDICOID AS MedicoId,
                PACIENTEID AS PacienteId,
                DATAHORACONSULTA AS DataHoraConsulta,
                VALOR AS Valor,
                STATUS AS Status,
                OBSERVACOES AS Observacoes,
                DATACADASTRO AS DataCadastro
            FROM CONSULTAS";

    private readonly DbConnectionFactory _dbFactory;
    private readonly ISqlDialect _dialect;

    public ConsultaDao(DbConnectionFactory dbFactory, DialectFactory dialectFactory)
    {
        _dbFactory = dbFactory;
        _dialect = dialectFactory.Criar();
    }

    private string P(string name) => _dialect.PrefixoParametro + name;

    public async Task<IEnumerable<Consulta>> ObterTodosAsync()
    {
        using var conn = _dbFactory.CreateOpenConnection();
        return await conn.QueryAsync<Consulta>(SelectColumns);
    }

    public async Task<Consulta?> ObterPorIdAsync(int id)
    {
        using var conn = _dbFactory.CreateOpenConnection();
        var sql = $"{SelectColumns}\n            WHERE ID = {P("Id")}";

        var parameters = new DynamicParameters();
        parameters.Add("Id", id, DbType.Int32);

        return await conn.QueryFirstOrDefaultAsync<Consulta>(sql, parameters);
    }

    public async Task<IEnumerable<ConsultaDto>> ObterTodosDetalhadosAsync()
    {
        using var conn = _dbFactory.CreateOpenConnection();

        var sql = @"
            SELECT 
                c.ID AS Id,
                c.MEDICOID AS MedicoId,
                c.PACIENTEID AS PacienteId,
                c.DATAHORACONSULTA AS DataHoraConsulta,
                c.VALOR AS Valor,
                c.STATUS AS Status,
                c.OBSERVACOES AS Observacoes,
                c.DATACADASTRO AS DataCadastro,
                m.NOME AS MedicoNome,
                p.NOME AS PacienteNome
            FROM CONSULTAS c
            INNER JOIN MEDICOS m ON m.ID = c.MEDICOID
            INNER JOIN PACIENTES p ON p.ID = c.PACIENTEID";

        return await conn.QueryAsync<ConsultaDto>(sql);
    }

    public async Task<int> InserirAsync(Consulta consulta)
    {
        using var conn = _dbFactory.CreateOpenConnection();

        var insertSql = $@"
            INSERT INTO CONSULTAS
            (MEDICOID, PACIENTEID, DATAHORACONSULTA, VALOR, STATUS, OBSERVACOES, DATACADASTRO)
            VALUES
            ({P("MedicoId")},
             {P("PacienteId")},
             {P("DataHoraConsulta")},
             {P("Valor")},
             {P("Status")},
             {P("Observacoes")},
             {P("DataCadastro")})";

        await conn.ExecuteAsync(insertSql, consulta);

        var selectIdSql = $@"
            SELECT ID
            FROM CONSULTAS
            WHERE MEDICOID = {P("MedicoId")}
              AND PACIENTEID = {P("PacienteId")}
              AND DATAHORACONSULTA = {P("DataHoraConsulta")}
              AND DATACADASTRO = {P("DataCadastro")}
            ORDER BY ID DESC
            {_dialect.Limite(1)}";

        var id = await conn.QuerySingleAsync<int>(selectIdSql, consulta);
        consulta.Id = id;
        return id;
    }

    public async Task<bool> AtualizarAsync(Consulta consulta)
    {
        using var conn = _dbFactory.CreateOpenConnection();

        var sql = @"
            UPDATE CONSULTAS
            SET MEDICOID = " + P("MedicoId") + @",
                PACIENTEID = " + P("PacienteId") + @",
                DATAHORACONSULTA = " + P("DataHoraConsulta") + @",
                VALOR = " + P("Valor") + @",
                STATUS = " + P("Status") + @",
                OBSERVACOES = " + P("Observacoes") + @"
            WHERE ID = " + P("Id");

        return await conn.ExecuteAsync(sql, consulta) > 0;
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        using var conn = _dbFactory.CreateOpenConnection();

        var sql = "DELETE FROM CONSULTAS WHERE ID = " + P("Id");

        var parameters = new DynamicParameters();
        parameters.Add("Id", id, DbType.Int32);

        return await conn.ExecuteAsync(sql, parameters) > 0;
    }
}
