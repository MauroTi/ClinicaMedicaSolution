namespace ClinicaMedica.Consumidor.ViewModels;

public class MedicoIndexViewModel
{
    public IReadOnlyList<MedicoViewModel> Medicos { get; set; } = Array.Empty<MedicoViewModel>();
    public PaginationViewModel Pagination { get; set; } = new();
}
