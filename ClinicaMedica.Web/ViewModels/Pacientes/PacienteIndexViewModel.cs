namespace ClinicaMedica.Web.ViewModels.Pacientes
{
    public class PacienteIndexViewModel
    {
        public IEnumerable<ClinicaMedica.Web.Models.Paciente> Pacientes { get; set; }
            = new List<ClinicaMedica.Web.Models.Paciente>();
    }
}