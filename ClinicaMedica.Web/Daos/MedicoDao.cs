using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.Data;
using ClinicaMedica.Web.Data.Dialects;
using ClinicaMedica.Web.Models;
using Dapper;

namespace ClinicaMedica.Web.Daos;

public class MedicoDao : IMedicoDao
{
    private readonly DbConnectionFactory _dbFactory;
    private readonly ISqlDialect _dialect;

    public MedicoDao(DbConnectionFactory dbFactory, DialectFactory dialectFactory)
    {
        _dbFactory = dbFactory;
        _dialect = dialectFactory.Criar();
    }

    public async Task<IEnumerable<Medico>> ObterTodos()
    {
        using var conn = _dbFactory.CreateOpenConnection();

        return await conn.QueryAsync<Medico>(
            "SELECT * FROM medicos ORDER BY Id"
        );
    }

    public async Task<Medico?> ObterPorId(int id)
    {
        using var conn = _dbFactory.CreateOpenConnection();

        var sql = $"SELECT * FROM medicos WHERE Id = {_dialect.PrefixoParametro}Id";

        return await conn.QueryFirstOrDefaultAsync<Medico>(sql, new { Id = id });
    }

    public async Task<Medico?> ObterPorCrmAsync(string crm)
    {
        using var conn = _dbFactory.CreateOpenConnection();

        var sql = $"SELECT * FROM medicos WHERE CRM = {_dialect.PrefixoParametro}Crm";

        return await conn.QueryFirstOrDefaultAsync<Medico>(sql, new { Crm = crm });
    }

    public async Task<bool> AdicionarAsync(Medico medico)
    {
        using var conn = _dbFactory.CreateOpenConnection();

        var sql = $@"
            INSERT INTO medicos
            (Nome, CRM, Especialidade, Telefone, Email, Ativo, DataCadastro)
            VALUES
            ({_dialect.PrefixoParametro}Nome,
             {_dialect.PrefixoParametro}Crm,
             {_dialect.PrefixoParametro}Especialidade,
             {_dialect.PrefixoParametro}Telefone,
             {_dialect.PrefixoParametro}Email,
             {_dialect.PrefixoParametro}Ativo,
             {_dialect.PrefixoParametro}DataCadastro)";

        return await conn.ExecuteAsync(sql, medico) > 0;
    }

    public async Task<bool> AtualizarAsync(Medico medico)
    {
        using var conn = _dbFactory.CreateOpenConnection();

        var sql = $@"
            UPDATE medicos SET
                Nome = {_dialect.PrefixoParametro}Nome,
                CRM = {_dialect.PrefixoParametro}Crm,
                Especialidade = {_dialect.PrefixoParametro}Especialidade,
                Telefone = {_dialect.PrefixoParametro}Telefone,
                Email = {_dialect.PrefixoParametro}Email,
                Ativo = {_dialect.PrefixoParametro}Ativo
            WHERE Id = {_dialect.PrefixoParametro}Id";

        return await conn.ExecuteAsync(sql, medico) > 0;
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        using var conn = _dbFactory.CreateOpenConnection();

        var sql = $"DELETE FROM medicos WHERE Id = {_dialect.PrefixoParametro}Id";

        return await conn.ExecuteAsync(sql, new { Id = id }) > 0;
    }
}