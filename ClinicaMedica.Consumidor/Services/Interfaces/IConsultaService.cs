using ClinicaMedica.Consumidor.ViewModels;

namespace ClinicaMedica.Consumidor.Services.Interfaces;

public interface IConsultaService
{
    Task<List<ConsultaViewModel>> ObterTodosAsync();
    Task<ConsultaViewModel?> ObterPorIdAsync(int id);
    Task<bool> CriarAsync(ConsultaViewModel model);
    Task<bool> AtualizarAsync(int id, ConsultaViewModel model);
    Task<bool> ExcluirAsync(int id);
    Task PreencherListasAsync(ConsultaViewModel model);
    Task<Dictionary<string, int>> ObterGraficoStatusAsync();
}
