// Models/Consulta.cs
namespace ClinicaMedica.Consumidor.Models
{
    public class Consulta
    {
        public int Id { get; set; }
        public int MedicoId { get; set; }
        public int PacienteId { get; set; }
        public DateTime DataHoraConsulta { get; set; }
        public decimal? Valor { get; set; }
        public string Status { get; set; } = null!;
        public string? Observacoes { get; set; }
        public DateTime DataCadastro { get; set; }

        public Medico? Medico { get; set; }
        public Paciente? Paciente { get; set; }
    }
}