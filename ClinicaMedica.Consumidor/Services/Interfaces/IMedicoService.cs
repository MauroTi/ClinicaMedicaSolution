using ClinicaMedica.Consumidor.ViewModels;

public interface IMedicoService
{
    Task<List<MedicoViewModel>> ObterTodosAsync();
    Task<MedicoViewModel?> ObterPorIdAsync(int id);
    Task<bool> CriarAsync(MedicoViewModel model);
    Task<bool> AtualizarAsync(int id, MedicoViewModel model);
    Task<bool> ExcluirAsync(int id);
}
