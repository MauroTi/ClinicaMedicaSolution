using ClinicaMedica.Web.Data;
using ClinicaMedica.Web.Data.Dialects;
using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.DTOs;
using ClinicaMedica.Web.Models;
using Dapper;

namespace ClinicaMedica.Web.Daos;

public class ConsultaDao : IConsultaDao
{
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
        return await conn.QueryAsync<Consulta>("SELECT * FROM consultas");
    }

    public async Task<Consulta?> ObterPorIdAsync(int id)
    {
        using var conn = _dbFactory.CreateOpenConnection();

        var sql = $"SELECT * FROM consultas WHERE Id = {_dialect.PrefixoParametro}Id";

        return await conn.QueryFirstOrDefaultAsync<Consulta>(sql, new { Id = id });
    }

    public async Task<IEnumerable<ConsultaDto>> ObterTodosDetalhadosAsync()
    {
        using var conn = _dbFactory.CreateOpenConnection();

        var sql = @"
            SELECT c.*, 
                   m.Nome AS MedicoNome,
                   p.Nome AS PacienteNome
            FROM consultas c
            INNER JOIN medicos m ON m.Id = c.MedicoId
            INNER JOIN pacientes p ON p.Id = c.PacienteId";

        return await conn.QueryAsync<ConsultaDto>(sql);
    }

    public async Task<int> InserirAsync(Consulta c)
    {
        using var conn = _dbFactory.CreateOpenConnection();

        var sql = $@"
            INSERT INTO consultas
            (MedicoId, PacienteId, DataHoraConsulta, Valor, Status, Observacoes, DataCadastro)
            VALUES
            ({P("MedicoId")},
             {P("PacienteId")},
             {P("DataHoraConsulta")},
             {P("Valor")},
             {P("Status")},
             {P("Observacoes")},
             {P("DataCadastro")})";

        return await conn.ExecuteAsync(sql, c);
    }

    public async Task<bool> AtualizarAsync(Consulta c)
    {
        using var conn = _dbFactory.CreateOpenConnection();

        var sql = $@"
            UPDATE consultas
            SET MedicoId = {P("MedicoId")},
                PacienteId = {P("PacienteId")},
                DataHoraConsulta = {P("DataHoraConsulta")},
                Valor = {P("Valor")},
                Status = {P("Status")},
                Observacoes = {P("Observacoes")}
            WHERE Id = {P("Id")}";

        return await conn.ExecuteAsync(sql, c) > 0;
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        using var conn = _dbFactory.CreateOpenConnection();

        var sql = $"DELETE FROM consultas WHERE Id = {_dialect.PrefixoParametro}Id";

        return await conn.ExecuteAsync(sql, new { Id = id }) > 0;
    }
}