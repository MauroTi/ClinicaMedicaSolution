namespace ClinicaMedica.Web.DTOs
{
    public class DashboardDto
    {
        public int TotalMedicos { get; set; }
        public int TotalPacientes { get; set; }
        public int TotalConsultas { get; set; }
        public int ConsultasAgendadas { get; set; }
        public int ConsultasRealizadas { get; set; }
        public int ConsultasCanceladas { get; set; }

        public decimal ValorTotalConsultas { get; set; }
        public decimal ValorTotalConsultasRealizadas { get; set; }

        public List<ConsultaDetalheDto> ProximasConsultas { get; set; } = new();
    }
}