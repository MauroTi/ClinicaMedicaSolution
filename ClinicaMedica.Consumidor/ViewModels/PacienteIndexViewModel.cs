namespace ClinicaMedica.Consumidor.ViewModels;

public class PacienteIndexViewModel
{
    public IReadOnlyList<PacienteViewModel> Pacientes { get; set; } = Array.Empty<PacienteViewModel>();
    public PaginationViewModel Pagination { get; set; } = new();
}
