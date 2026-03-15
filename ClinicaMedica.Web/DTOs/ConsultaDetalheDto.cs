namespace ClinicaMedica.Web.DTOs
{
    public class ConsultaDetalheDto
    {
        public int Id { get; set; }

        public int MedicoId { get; set; }
        public string NomeMedico { get; set; } = string.Empty;
        public string EspecialidadeMedico { get; set; } = string.Empty;

        public int PacienteId { get; set; }
        public string NomePaciente { get; set; } = string.Empty;
        public string CpfPaciente { get; set; } = string.Empty;

        public DateTime DataHoraConsulta { get; set; }
        public decimal Valor { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Observacoes { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}