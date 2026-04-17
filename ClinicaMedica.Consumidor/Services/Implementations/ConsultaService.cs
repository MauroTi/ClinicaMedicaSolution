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
        var consulta = await _api.GetByIdAsync<ConsultaViewModel>(_endpoint, id);

        if (consulta == null)
        {
            return null;
        }

        if (consulta.MedicoId > 0)
        {
            var medico = await _api.GetByIdAsync<MedicoViewModel>("medicosApi", consulta.MedicoId);
            consulta.NomeMedico = medico?.Nome;
        }

        if (consulta.PacienteId > 0)
        {
            var paciente = await _api.GetByIdAsync<PacienteViewModel>("pacientes", consulta.PacienteId);
            consulta.NomePaciente = paciente?.Nome;
        }

        return consulta;
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
        model.Medicos = (await _api.GetAllAsync<MedicoViewModel>("medicosApi"))
            .OrderBy(m => m.Nome)
            .ToList();

        model.Pacientes = (await _api.GetAllAsync<PacienteViewModel>("pacientes"))
            .OrderBy(p => p.Nome)
            .ToList();
    }

    public async Task<Dictionary<string, int>> ObterGraficoStatusAsync()
    {
        var dados = await _api.GetAllAsync<ConsultaViewModel>(_endpoint);

        return dados
            .GroupBy(c => string.IsNullOrWhiteSpace(c.Status) ? "Sem status" : c.Status.Trim())
            .ToDictionary(g => g.Key, g => g.Count());
    }
}
