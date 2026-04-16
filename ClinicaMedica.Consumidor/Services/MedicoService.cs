using ClinicaMedica.Consumidor.ViewModels;

public class MedicoService : IMedicoService
{
    private readonly IApiService _api;
    private readonly string _endpoint = "medicos";

    public MedicoService(IApiService api)
    {
        _api = api;
    }

    public async Task<List<MedicoViewModel>> ObterTodosAsync()
        => await _api.GetAllAsync<MedicoViewModel>(_endpoint);

    public async Task<MedicoViewModel> ObterPorIdAsync(int id)
        => await _api.GetByIdAsync<MedicoViewModel>(_endpoint, id);

    public async Task CriarAsync(MedicoViewModel model)
        => await _api.PostAsync<MedicoViewModel>(_endpoint, model);
}