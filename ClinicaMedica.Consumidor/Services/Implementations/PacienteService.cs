using ClinicaMedica.Consumidor.Services.Interfaces;
using ClinicaMedica.Consumidor.ViewModels;

namespace ClinicaMedica.Consumidor.Services.Implementations;

public class PacienteService : CrudApiServiceBase<PacienteViewModel>, IPacienteService
{
    public PacienteService(IApiService api)
        : base(api, ApiEndpoints.Pacientes)
    {
    }

    public Task<List<PacienteViewModel>> ObterTodosAsync()
        => GetAllAsync();

    public Task<PacienteViewModel?> ObterPorIdAsync(int id)
        => GetByIdAsync(id);

    public Task<bool> CriarAsync(PacienteViewModel model)
        => CreateAsync(model);

    public Task<bool> AtualizarAsync(int id, PacienteViewModel model)
        => UpdateAsync(id, model);

    public Task<bool> ExcluirAsync(int id)
        => DeleteAsync(id);
}
