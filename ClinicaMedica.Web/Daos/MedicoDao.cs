using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.Data;
using ClinicaMedica.Web.Data.Dialects;
using ClinicaMedica.Web.Models;
using Dapper;
using System.Data;

namespace ClinicaMedica.Web.Daos;

public class MedicoDao : IMedicoDao
{
    private const string SelectColumns = @"
            SELECT 
                ID AS Id,
                NOME AS Nome,
                CRM AS Crm,
                ESPECIALIDADE AS Especialidade,
                TELEFONE AS Telefone,
                EMAIL AS Email,
                ATIVO AS Ativo,
                DATACADASTRO AS DataCadastro
            FROM MEDICOS";

    private readonly DbConnectionFactory _dbFactory;
    private readonly ISqlDialect _dialect;

    public MedicoDao(DbConnectionFactory dbFactory, DialectFactory dialectFactory)
    {
        _dbFactory = dbFactory;
        _dialect = dialectFactory.Criar();
    }

    private string P(string name) => _dialect.PrefixoParametro + name;

    public async Task<IEnumerable<Medico>> ObterTodos()
    {
        using var conn = _dbFactory.CreateOpenConnection();
        var sql = $"{SelectColumns}\n            ORDER BY ID";

        return await conn.QueryAsync<Medico>(sql);
    }

    public async Task<Medico?> ObterPorId(int id)
    {
        using var conn = _dbFactory.CreateOpenConnection();
        var sql = $"{SelectColumns}\n            WHERE ID = {P("Id")}";

        var parameters = new DynamicParameters();
        parameters.Add("Id", id, DbType.Int32);

        return await conn.QueryFirstOrDefaultAsync<Medico>(sql, parameters);
    }

    public async Task<Medico?> ObterPorCrmAsync(string crm)
    {
        using var conn = _dbFactory.CreateOpenConnection();
        var sql = $"{SelectColumns}\n            WHERE CRM = {P("Crm")}";

        var parameters = new DynamicParameters();
        parameters.Add("Crm", crm, DbType.String);

        return await conn.QueryFirstOrDefaultAsync<Medico>(sql, parameters);
    }

    public async Task<bool> AdicionarAsync(Medico medico)
    {
        using var conn = _dbFactory.CreateOpenConnection();

        var sql = $@"
            INSERT INTO MEDICOS
            (NOME, CRM, ESPECIALIDADE, TELEFONE, EMAIL, ATIVO, DATACADASTRO)
            VALUES
            ({P("Nome")},
             {P("Crm")},
             {P("Especialidade")},
             {P("Telefone")},
             {P("Email")},
             {P("Ativo")},
             {P("DataCadastro")})";

        return await conn.ExecuteAsync(sql, CreateMedicoParameters(medico)) > 0;
    }

    public async Task<bool> AtualizarAsync(Medico medico)
    {
        using var conn = _dbFactory.CreateOpenConnection();

        var sql = @"
            UPDATE MEDICOS SET
                NOME = " + P("Nome") + @",
                CRM = " + P("Crm") + @",
                ESPECIALIDADE = " + P("Especialidade") + @",
                TELEFONE = " + P("Telefone") + @",
                EMAIL = " + P("Email") + @",
                ATIVO = " + P("Ativo") + @"
            WHERE ID = " + P("Id");

        return await conn.ExecuteAsync(sql, CreateMedicoParameters(medico)) > 0;
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        using var conn = _dbFactory.CreateOpenConnection();

        var sql = "DELETE FROM MEDICOS WHERE ID = " + P("Id");

        var parameters = new DynamicParameters();
        parameters.Add("Id", id, DbType.Int32);

        return await conn.ExecuteAsync(sql, parameters) > 0;
    }

    private DynamicParameters CreateMedicoParameters(Medico medico)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Id", medico.Id, DbType.Int32);
        parameters.Add("Nome", medico.Nome, DbType.String);
        parameters.Add("Crm", medico.Crm, DbType.String);
        parameters.Add("Especialidade", medico.Especialidade, DbType.String);
        parameters.Add("Telefone", medico.Telefone, DbType.String);
        parameters.Add("Email", medico.Email, DbType.String);
        parameters.Add("Ativo", NormalizeBoolean(medico.Ativo), DbType.Int32);
        parameters.Add("DataCadastro", medico.DataCadastro, DbType.DateTime);
        return parameters;
    }

    private int NormalizeBoolean(bool value)
    {
        return value ? 1 : 0;
    }
}
