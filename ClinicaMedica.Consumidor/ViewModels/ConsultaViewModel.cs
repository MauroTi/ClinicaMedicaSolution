// ViewModels/ConsultaViewModel.cs
using ClinicaMedica.Consumidor.Models;

namespace ClinicaMedica.Consumidor.ViewModels
{
    public class ConsultaViewModel
    {
        public int Id { get; set; }
        public int MedicoId { get; set; }
        public int PacienteId { get; set; }
        public DateTime DataHoraConsulta { get; set; }
        public decimal? Valor { get; set; }
        public string Status { get; set; } = null!;
        public string? Observacoes { get; set; }

        public string? NomeMedico { get; set; }
        public string? NomePaciente { get; set; }

        public List<Medico> Medicos { get; set; } = new List<Medico>();
        public List<Paciente> Pacientes { get; set; } = new List<Paciente>();
    }
}