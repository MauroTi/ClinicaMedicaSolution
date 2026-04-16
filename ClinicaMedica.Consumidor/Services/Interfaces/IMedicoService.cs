using ClinicaMedica.Consumidor.ViewModels;

public interface IMedicoService
{
    Task<List<MedicoViewModel>> ObterTodosAsync();
    Task<MedicoViewModel> ObterPorIdAsync(int id);
    Task CriarAsync(MedicoViewModel model);
}