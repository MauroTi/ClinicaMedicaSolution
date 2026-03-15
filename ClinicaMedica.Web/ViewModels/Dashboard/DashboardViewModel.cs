// Path: ViewModels/Dashboard/DashboardViewModel.cs
namespace ClinicaMedica.Web.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public int TotalMedicos { get; set; }
        public int TotalPacientes { get; set; }
        public int TotalConsultas { get; set; }
        public decimal ReceitaTotal { get; set; }

        public required string ConsultasAgendadas { get; set; }
        // Outras métricas que a view precisa
    }
}