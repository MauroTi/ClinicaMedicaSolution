namespace ClinicaMedica.Web.ViewModels.Dashboard
{
    public class DashboardResumoViewModel
    {
        public int TotalPacientes { get; set; }
        public int TotalMedicos { get; set; }
        public int TotalConsultas { get; set; }
        public decimal ReceitaTotal { get; internal set; }
        public string ConsultasAgendadas { get; internal set; } = string.Empty;
    }
}
