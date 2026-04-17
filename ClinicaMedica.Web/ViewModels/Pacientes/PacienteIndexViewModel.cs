using ClinicaMedica.Web.ViewModels.Shared;

namespace ClinicaMedica.Web.ViewModels.Pacientes;

public class PacienteIndexViewModel
{
    public List<ClinicaMedica.Web.Models.Paciente> Pacientes { get; set; } = new();
    public PaginationViewModel Pagination { get; set; } = new();
}
