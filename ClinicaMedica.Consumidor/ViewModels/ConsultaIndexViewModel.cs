namespace ClinicaMedica.Consumidor.ViewModels;

public class ConsultaIndexViewModel
{
    public IReadOnlyList<ConsultaViewModel> Consultas { get; set; } = Array.Empty<ConsultaViewModel>();
    public PaginationViewModel Pagination { get; set; } = new();
}
