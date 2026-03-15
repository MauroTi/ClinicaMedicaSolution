namespace ClinicaMedica.Web.ViewModels.Dashboard
{
    public class DashboardIndexViewModel
    {
        public int TotalPacientes { get; set; }
        public int TotalMedicos { get; set; }
        public int TotalConsultas { get; set; }
        public int ReceitaTotal { get; set; }
        public required string ConsultasAgendadas { get; set; }
    }
}