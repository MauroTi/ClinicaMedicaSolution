using ClinicaMedica.Consumidor.Services.Interfaces;

namespace ClinicaMedica.Consumidor.Services.Implementations;

public abstract class CrudApiServiceBase<TViewModel>
{
    private readonly IApiService _apiService;
    private readonly string _endpoint;

    protected CrudApiServiceBase(IApiService apiService, string endpoint)
    {
        _apiService = apiService;
        _endpoint = endpoint;
    }

    protected Task<List<TViewModel>> GetAllAsync()
        => _apiService.GetAllAsync<TViewModel>(_endpoint);

    protected Task<TViewModel?> GetByIdAsync(int id)
        => _apiService.GetByIdAsync<TViewModel>(_endpoint, id);

    protected Task<bool> CreateAsync(TViewModel model)
        => _apiService.PostAsync(_endpoint, model);

    protected Task<bool> UpdateAsync(int id, TViewModel model)
        => _apiService.PutAsync(_endpoint, id, model);

    protected Task<bool> DeleteAsync(int id)
        => _apiService.DeleteAsync(_endpoint, id);
}
