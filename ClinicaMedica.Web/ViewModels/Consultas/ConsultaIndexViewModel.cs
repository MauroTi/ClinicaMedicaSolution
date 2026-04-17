using ClinicaMedica.Web.DTOs;
using ClinicaMedica.Web.ViewModels.Shared;

namespace ClinicaMedica.Web.ViewModels.Consultas;

public class ConsultaIndexViewModel
{
    public int Id { get; set; }
    public string MedicoNome { get; set; } = string.Empty;
    public string PacienteNome { get; set; } = string.Empty;
    public DateTime DataHoraConsulta { get; set; }
    public decimal Valor { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Observacoes { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; }
    public List<ConsultaDto> Consultas { get; set; } = new();
    public PaginationViewModel Pagination { get; set; } = new();
}
