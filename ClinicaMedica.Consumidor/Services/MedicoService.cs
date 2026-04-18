using ClinicaMedica.Consumidor.Services.Interfaces;
using ClinicaMedica.Consumidor.ViewModels;
using ClinicaMedica.Consumidor.Services.Implementations;

namespace ClinicaMedica.Consumidor.Services;

public class MedicoService : CrudApiServiceBase<MedicoViewModel>, IMedicoService
{
    public MedicoService(IApiService api)
        : base(api, ApiEndpoints.Medicos)
    {
    }

    public Task<List<MedicoViewModel>> ObterTodosAsync()
        => GetAllAsync();

    public Task<MedicoViewModel?> ObterPorIdAsync(int id)
        => GetByIdAsync(id);

    public Task<bool> CriarAsync(MedicoViewModel model)
        => CreateAsync(model);

    public Task<bool> AtualizarAsync(int id, MedicoViewModel model)
        => UpdateAsync(id, model);

    public Task<bool> ExcluirAsync(int id)
        => DeleteAsync(id);
}
