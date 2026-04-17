using ClinicaMedica.Consumidor.ViewModels;

public class MedicoService : IMedicoService
{
    private readonly IApiService _api;
    private const string Endpoint = "medicosApi";

    public MedicoService(IApiService api)
    {
        _api = api;
    }

    public async Task<List<MedicoViewModel>> ObterTodosAsync()
        => await _api.GetAllAsync<MedicoViewModel>(Endpoint);

    public async Task<MedicoViewModel?> ObterPorIdAsync(int id)
        => await _api.GetByIdAsync<MedicoViewModel>(Endpoint, id);

    public async Task<bool> CriarAsync(MedicoViewModel model)
        => await _api.PostAsync(Endpoint, model);

    public async Task<bool> AtualizarAsync(int id, MedicoViewModel model)
        => await _api.PutAsync(Endpoint, id, model);

    public async Task<bool> ExcluirAsync(int id)
        => await _api.DeleteAsync(Endpoint, id);
}
