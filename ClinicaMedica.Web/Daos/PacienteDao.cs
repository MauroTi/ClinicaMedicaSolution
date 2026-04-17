using ClinicaMedica.Web.Data;
using ClinicaMedica.Web.Data.Dialects;
using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.Models;
using Dapper;

namespace ClinicaMedica.Web.Daos;

public class PacienteDao : IPacienteDao
{
    private readonly DbConnectionFactory _dbFactory;
    private readonly ISqlDialect _dialect;

    public PacienteDao(DbConnectionFactory dbFactory, DialectFactory dialectFactory)
    {
        _dbFactory = dbFactory;
        _dialect = dialectFactory.Criar();
    }

    public async Task<IEnumerable<Paciente>> ObterTodos()
    {
        using var conn = _dbFactory.CreateOpenConnection();
        return await conn.QueryAsync<Paciente>("SELECT * FROM pacientes ORDER BY Id");
    }

    public async Task<Paciente?> ObterPorId(int id)
    {
        using var conn = _dbFactory.CreateOpenConnection();

        var sql = $"SELECT * FROM pacientes WHERE Id = {_dialect.PrefixoParametro}Id";

        return await conn.QueryFirstOrDefaultAsync<Paciente>(sql, new { Id = id });
    }

    public async Task CriarAsync(Paciente model)
    {
        using var conn = _dbFactory.CreateOpenConnection();

        var sql = $@"
            INSERT INTO pacientes
            (Nome, Cpf, Telefone, Email, DataNascimento, Ativo, DataCadastro)
            VALUES
            ({_dialect.PrefixoParametro}Nome,
             {_dialect.PrefixoParametro}Cpf,
             {_dialect.PrefixoParametro}Telefone,
             {_dialect.PrefixoParametro}Email,
             {_dialect.PrefixoParametro}DataNascimento,
             {_dialect.PrefixoParametro}Ativo,
             {_dialect.PrefixoParametro}DataCadastro)";

        await conn.ExecuteAsync(sql, model);
    }

    public async Task<bool> EditarAsync(Paciente model)
    {
        using var conn = _dbFactory.CreateOpenConnection();

        var sql = $@"
            UPDATE pacientes
            SET Nome = {_dialect.PrefixoParametro}Nome,
                Cpf = {_dialect.PrefixoParametro}Cpf,
                Telefone = {_dialect.PrefixoParametro}Telefone,
                Email = {_dialect.PrefixoParametro}Email,
                DataNascimento = {_dialect.PrefixoParametro}DataNascimento,
                Ativo = {_dialect.PrefixoParametro}Ativo
            WHERE Id = {_dialect.PrefixoParametro}Id";

        return await conn.ExecuteAsync(sql, model) > 0;
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        using var conn = _dbFactory.CreateOpenConnection();

        var sql = $"DELETE FROM pacientes WHERE Id = {_dialect.PrefixoParametro}Id";

        return await conn.ExecuteAsync(sql, new { Id = id }) > 0;
    }

    public async Task<bool> ExisteCpfAsync(string cpf)
    {
        using var conn = _dbFactory.CreateOpenConnection();

        const string sql = "SELECT COUNT(1) FROM pacientes WHERE Cpf = @Cpf";

        return await conn.ExecuteScalarAsync<int>(sql, new { Cpf = cpf }) > 0;
    }
}