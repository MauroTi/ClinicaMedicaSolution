namespace ClinicaMedica.Web.DTOs
{
    public class ConsultaDto
    {
        public int Id { get; set; }
        public int MedicoId { get; set; }
        public int PacienteId { get; set; }
        public DateTime DataHoraConsulta { get; set; }
        public decimal Valor { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Observacoes { get; set; }
        public DateTime DataCadastro { get; set; }

        // CAMPOS DETALHADOS (vindos do JOIN)
        public string MedicoNome { get; set; } = string.Empty;
        public string PacienteNome { get; set; } = string.Empty;
    }
}