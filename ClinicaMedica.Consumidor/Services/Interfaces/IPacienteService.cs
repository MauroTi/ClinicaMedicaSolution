using ClinicaMedica.Consumidor.ViewModels;

namespace ClinicaMedica.Consumidor.Services.Interfaces;

public interface IPacienteService
{
    Task<List<PacienteViewModel>> ObterTodosAsync();
    Task<PacienteViewModel?> ObterPorIdAsync(int id);
    Task<bool> CriarAsync(PacienteViewModel model);
    Task<bool> AtualizarAsync(int id, PacienteViewModel model);
    Task<bool> ExcluirAsync(int id);
}
