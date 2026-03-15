using System.Collections.Generic;

namespace ClinicaMedica.Web.ViewModels.Pacientes
{
    public class PacienteIndexViewModel
    {
        // Use o namespace completo para evitar conflito com o namespace Paciente
        public List<ClinicaMedica.Web.Models.Paciente> Pacientes { get; set; } = new List<ClinicaMedica.Web.Models.Paciente>();
    }
}