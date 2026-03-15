namespace ClinicaMedica.Web.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public int TotalMedicos { get; set; }
        public int TotalPacientes { get; set; }
        public int TotalConsultas { get; set; }
        public decimal ReceitaTotal { get; set; }

        public int ConsultasAgendadas { get; set; }
        public int ConsultasRealizadas { get; set; }
        public int ConsultasCanceladas { get; set; }
    }
}