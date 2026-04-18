using ClinicaMedica.Web.Data;
using ClinicaMedica.Web.Data.Dialects;
using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.Models;
using Dapper;
using System.Data;

namespace ClinicaMedica.Web.Daos;

public class PacienteDao : IPacienteDao
{
    private const string SelectColumns = @"
            SELECT 
                ID AS Id,
                NOME AS Nome,
                CPF AS Cpf,
                TELEFONE AS Telefone,
                EMAIL AS Email,
                DATANASCIMENTO AS DataNascimento,
                ATIVO AS Ativo,
                DATACADASTRO AS DataCadastro
            FROM PACIENTES";

    private readonly DbConnectionFactory _dbFactory;
    private readonly ISqlDialect _dialect;

    public PacienteDao(DbConnectionFactory dbFactory, DialectFactory dialectFactory)
    {
        _dbFactory = dbFactory;
        _dialect = dialectFactory.Criar();
    }

    private string P(string name) => _dialect.PrefixoParametro + name;

    public async Task<IEnumerable<Paciente>> ObterTodos()
    {
        using var conn = _dbFactory.CreateOpenConnection();
        var sql = $"{SelectColumns}\n            ORDER BY ID";

        return await conn.QueryAsync<Paciente>(sql);
    }

    public async Task<Paciente?> ObterPorId(int id)
    {
        using var conn = _dbFactory.CreateOpenConnection();
        var sql = $"{SelectColumns}\n            WHERE ID = {P("Id")}";

        var parameters = new DynamicParameters();
        parameters.Add("Id", id, DbType.Int32);

        return await conn.QueryFirstOrDefaultAsync<Paciente>(sql, parameters);
    }

    public async Task<Paciente?> ObterPorCpfAsync(string cpf)
    {
        using var conn = _dbFactory.CreateOpenConnection();
        var sql = $"{SelectColumns}\n            WHERE CPF = {P("Cpf")}";

        var parameters = new DynamicParameters();
        parameters.Add("Cpf", cpf, DbType.String);

        return await conn.QueryFirstOrDefaultAsync<Paciente>(sql, parameters);
    }

    public async Task<int> CriarAsync(Paciente model)
    {
        using var conn = _dbFactory.CreateOpenConnection();

        var insertSql = $@"
            INSERT INTO PACIENTES
            (NOME, CPF, TELEFONE, EMAIL, DATANASCIMENTO, ATIVO, DATACADASTRO)
            VALUES
            ({P("Nome")},
             {P("Cpf")},
             {P("Telefone")},
             {P("Email")},
             {P("DataNascimento")},
             {P("Ativo")},
             {P("DataCadastro")})";

        await conn.ExecuteAsync(insertSql, CreatePacienteParameters(model));

        var selectIdSql = $@"
            SELECT ID
            FROM PACIENTES
            WHERE CPF = {P("Cpf")}
            ORDER BY ID DESC
            {_dialect.Limite(1)}";

        var parameters = new DynamicParameters();
        parameters.Add("Cpf", model.Cpf, DbType.String);

        return await conn.QuerySingleAsync<int>(selectIdSql, parameters);
    }

    public async Task<bool> EditarAsync(Paciente model)
    {
        using var conn = _dbFactory.CreateOpenConnection();

        var sql = @"
            UPDATE PACIENTES
            SET NOME = " + P("Nome") + @",
                CPF = " + P("Cpf") + @",
                TELEFONE = " + P("Telefone") + @",
                EMAIL = " + P("Email") + @",
                DATANASCIMENTO = " + P("DataNascimento") + @",
                ATIVO = " + P("Ativo") + @"
            WHERE ID = " + P("Id");

        return await conn.ExecuteAsync(sql, CreatePacienteParameters(model)) > 0;
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        using var conn = _dbFactory.CreateOpenConnection();

        var sql = "DELETE FROM PACIENTES WHERE ID = " + P("Id");

        var parameters = new DynamicParameters();
        parameters.Add("Id", id, DbType.Int32);

        return await conn.ExecuteAsync(sql, parameters) > 0;
    }

    public async Task<bool> ExisteCpfAsync(string cpf)
    {
        using var conn = _dbFactory.CreateOpenConnection();

        var sql = "SELECT COUNT(1) FROM PACIENTES WHERE CPF = " + P("Cpf");

        var parameters = new DynamicParameters();
        parameters.Add("Cpf", cpf, DbType.String);

        return await conn.ExecuteScalarAsync<int>(sql, parameters) > 0;
    }

    private DynamicParameters CreatePacienteParameters(Paciente model)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Id", model.Id, DbType.Int32);
        parameters.Add("Nome", model.Nome, DbType.String);
        parameters.Add("Cpf", model.Cpf, DbType.String);
        parameters.Add("Telefone", model.Telefone, DbType.String);
        parameters.Add("Email", model.Email, DbType.String);
        parameters.Add("DataNascimento", model.DataNascimento, DbType.DateTime);
        parameters.Add("Ativo", NormalizeBoolean(model.Ativo), DbType.Int32);
        parameters.Add("DataCadastro", model.DataCadastro, DbType.DateTime);
        return parameters;
    }

    private int NormalizeBoolean(bool value)
    {
        return value ? 1 : 0;
    }
}
