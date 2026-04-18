using ClinicaMedica.Consumidor.ViewModels;

namespace ClinicaMedica.Consumidor.Services.Interfaces;

public interface IMedicoService
{
    Task<List<MedicoViewModel>> ObterTodosAsync();
    Task<MedicoViewModel?> ObterPorIdAsync(int id);
    Task<bool> CriarAsync(MedicoViewModel model);
    Task<bool> AtualizarAsync(int id, MedicoViewModel model);
    Task<bool> ExcluirAsync(int id);
}
