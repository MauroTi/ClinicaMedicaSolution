namespace ClinicaMedica.Web.DTOs
{
    public class DashboardResumoDto
    {
        public int TotalPacientesAtivos { get; set; }
        public int TotalMedicosAtivos { get; set; }
        public int TotalConsultas { get; set; }
        public int ConsultasAgendadas { get; set; }
        public int ConsultasRealizadas { get; set; }
        public int ConsultasCanceladas { get; set; }
        public int ConsultasHoje { get; set; }
    }
}