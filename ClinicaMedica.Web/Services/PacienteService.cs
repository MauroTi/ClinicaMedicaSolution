using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.Infrastructure.Database;
using ClinicaMedica.Web.Models;
using ClinicaMedica.Web.Services.Interfaces;

namespace ClinicaMedica.Web.Services;

public class PacienteService : IPacienteService
{
    private readonly IPacienteDao _pacienteDao;

    public PacienteService(IPacienteDao pacienteDao)
    {
        _pacienteDao = pacienteDao;
    }

    public Task<IEnumerable<Paciente>> ObterTodosAsync() => _pacienteDao.ObterTodos();

    public Task<Paciente?> ObterPorIdAsync(int id) => _pacienteDao.ObterPorId(id);

    public async Task<bool> ExcluirAsync(int id)
    {
        try
        {
            return await _pacienteDao.ExcluirAsync(id);
        }
        catch (Exception ex) when (DatabaseExceptionTranslator.IsReferenceConstraintViolation(ex))
        {
            throw new InvalidOperationException("Não é possível excluir o paciente porque existem consultas vinculadas a ele.");
        }
    }

    public Task<bool> ExisteCpfAsync(string cpf) => _pacienteDao.ExisteCpfAsync(cpf);

    public async Task<int> AdicionarAsync(Paciente model)
    {
        if (model.DataCadastro == default)
            model.DataCadastro = DateTime.Now;

        var existente = await _pacienteDao.ObterPorCpfAsync(model.Cpf);
        if (existente is not null)
            throw new InvalidOperationException($"O CPF {model.Cpf} já está cadastrado.");

        var id = await _pacienteDao.CriarAsync(model);
        model.Id = id;
        return id;
    }

    public async Task<bool> AtualizarAsync(Paciente model)
    {
        if (model.DataCadastro == default)
            model.DataCadastro = DateTime.Now;

        var existente = await _pacienteDao.ObterPorCpfAsync(model.Cpf);
        if (existente is not null && existente.Id != model.Id)
            throw new InvalidOperationException($"O CPF {model.Cpf} já está cadastrado para outro paciente.");

        return await _pacienteDao.EditarAsync(model);
    }
}
