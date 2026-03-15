namespace ClinicaMedica.Web.Daos.Interfaces
{
    public interface IDashboardDao
    {
        int ObterTotalMedicos();
        int ObterTotalPacientes();
        int ObterTotalConsultas();
        decimal ObterReceitaTotal();
        int ConsultasAgendadas(); // apenas a assinatura
    }
}