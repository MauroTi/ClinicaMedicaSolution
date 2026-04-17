using ClinicaMedica.Web.Daos.Interfaces;
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

    public async Task<IEnumerable<Paciente>> ObterTodosAsync()
    {
        return await _pacienteDao.ObterTodos();
    }

    public async Task<Paciente?> ObterPorIdAsync(int id)
    {
        return await _pacienteDao.ObterPorId(id);
    }

    public async Task CriarAsync(Paciente model)
    {
        await _pacienteDao.CriarAsync(model);
    }

    public async Task<bool> EditarAsync(Paciente model)
    {
        return await _pacienteDao.EditarAsync(model);
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        return await _pacienteDao.ExcluirAsync(id);
    }

    public async Task<bool> ExisteCpfAsync(string cpf)
    {
        return await _pacienteDao.ExisteCpfAsync(cpf);
    }

    public Task<int> AdicionarAsync(Paciente model)
    {
        return _pacienteDao.CriarAsync(model)
            .ContinueWith(_ => 1);
    }

    public Task<bool> AtualizarAsync(Paciente model)
    {
        return _pacienteDao.EditarAsync(model);
    }
}