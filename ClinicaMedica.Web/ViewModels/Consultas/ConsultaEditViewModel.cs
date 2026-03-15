using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.Web.ViewModels.Consultas
{
    public class ConsultaEditViewModel
    {
        public int Id { get; set; }

        [Required]
        public int PacienteId { get; set; }

        [Required]
        public int MedicoId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DataHoraConsulta { get; set; }

        [Required]
        public string Status { get; set; } = "Agendada";

        public string? Observacoes { get; set; }

        public IEnumerable<PacienteOptionViewModel> Pacientes { get; set; } = new List<PacienteOptionViewModel>();
        public IEnumerable<MedicoOptionViewModel> Medicos { get; set; } = new List<MedicoOptionViewModel>();
    }
}