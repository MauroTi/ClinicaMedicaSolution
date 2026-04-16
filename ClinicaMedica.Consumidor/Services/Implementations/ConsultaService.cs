using ClinicaMedica.Consumidor.Services.Interfaces;
using ClinicaMedica.Consumidor.ViewModels;

namespace ClinicaMedica.Consumidor.Services.Implementations;

public class ConsultaService : IConsultaService
{
    private readonly IApiService _api;
    private readonly string _endpoint = "consultas";

    public ConsultaService(IApiService api)
    {
        _api = api;
    }

    public async Task<List<ConsultaViewModel>> ObterTodosAsync()
    {
        return await _api.GetAllAsync<ConsultaViewModel>(_endpoint);
    }

    public async Task<ConsultaViewModel?> ObterPorIdAsync(int id)
    {
        return await _api.GetByIdAsync<ConsultaViewModel>(_endpoint, id);
    }

    public async Task<bool> CriarAsync(ConsultaViewModel model)
    {
        return await _api.PostAsync(_endpoint, model);
    }

    public async Task<bool> AtualizarAsync(int id, ConsultaViewModel model)
    {
        return await _api.PutAsync(_endpoint, id, model);
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        return await _api.DeleteAsync(_endpoint, id);
    }

    public async Task PreencherListasAsync(ConsultaViewModel model)
    {
        // 🔥 Ajuste os tipos se você tiver ViewModels específicos
        model.Medicos = await _api.GetAllAsync<MedicoViewModel>("medicos");
        model.Pacientes = await _api.GetAllAsync<PacienteViewModel>("pacientes");
    }

    public async Task<object> ObterGraficoStatusAsync()
    {
        return await _api.GetAllAsync<object>("consultas/grafico-status");
    }
}