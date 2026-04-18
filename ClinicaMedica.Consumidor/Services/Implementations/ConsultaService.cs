using ClinicaMedica.Consumidor.Services.Interfaces;
using ClinicaMedica.Consumidor.ViewModels;

namespace ClinicaMedica.Consumidor.Services.Implementations;

public class ConsultaService : IConsultaService
{
    private readonly IApiService _api;
    private readonly IMedicoService _medicoService;
    private readonly IPacienteService _pacienteService;

    public ConsultaService(
        IApiService api,
        IMedicoService medicoService,
        IPacienteService pacienteService)
    {
        _api = api;
        _medicoService = medicoService;
        _pacienteService = pacienteService;
    }

    public Task<List<ConsultaViewModel>> ObterTodosAsync()
    {
        return _api.GetAllAsync<ConsultaViewModel>(ApiEndpoints.Consultas);
    }

    public async Task<ConsultaViewModel?> ObterPorIdAsync(int id)
    {
        var consulta = await _api.GetByIdAsync<ConsultaViewModel>(ApiEndpoints.Consultas, id);

        if (consulta == null)
            return null;

        await EnrichRelatedNamesAsync(consulta);
        return consulta;
    }

    public Task<bool> CriarAsync(ConsultaViewModel model)
    {
        return _api.PostAsync(ApiEndpoints.Consultas, model);
    }

    public Task<bool> AtualizarAsync(int id, ConsultaViewModel model)
    {
        return _api.PutAsync(ApiEndpoints.Consultas, id, model);
    }

    public Task<bool> ExcluirAsync(int id)
    {
        return _api.DeleteAsync(ApiEndpoints.Consultas, id);
    }

    public async Task PreencherListasAsync(ConsultaViewModel model)
    {
        var medicosTask = _medicoService.ObterTodosAsync();
        var pacientesTask = _pacienteService.ObterTodosAsync();

        await Task.WhenAll(medicosTask, pacientesTask);

        model.Medicos = medicosTask.Result
            .OrderBy(m => m.Nome)
            .ToList();

        model.Pacientes = pacientesTask.Result
            .OrderBy(p => p.Nome)
            .ToList();
    }

    public async Task<Dictionary<string, int>> ObterGraficoStatusAsync()
    {
        return await _api.GetAsync<Dictionary<string, int>>(ApiEndpoints.ConsultasGraficoStatus)
            ?? [];
    }

    private async Task EnrichRelatedNamesAsync(ConsultaViewModel consulta)
    {
        var medicoTask = NeedsDoctorLookup(consulta)
            ? _medicoService.ObterPorIdAsync(consulta.MedicoId)
            : Task.FromResult<MedicoViewModel?>(null);

        var pacienteTask = NeedsPatientLookup(consulta)
            ? _pacienteService.ObterPorIdAsync(consulta.PacienteId)
            : Task.FromResult<PacienteViewModel?>(null);

        await Task.WhenAll(medicoTask, pacienteTask);

        consulta.NomeMedico ??= medicoTask.Result?.Nome;
        consulta.NomePaciente ??= pacienteTask.Result?.Nome;
    }

    private static bool NeedsDoctorLookup(ConsultaViewModel consulta)
        => string.IsNullOrWhiteSpace(consulta.NomeMedico) && consulta.MedicoId > 0;

    private static bool NeedsPatientLookup(ConsultaViewModel consulta)
        => string.IsNullOrWhiteSpace(consulta.NomePaciente) && consulta.PacienteId > 0;
}
