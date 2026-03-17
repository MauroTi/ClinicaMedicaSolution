namespace ClinicaMedica.Consumidor.Models
{
    public class ConsultaListaItem
    {
        public int Id { get; set; }

        public string MedicoNome { get; set; } = string.Empty;

        public string PacienteNome { get; set; } = string.Empty;

        public DateTime DataHoraConsulta { get; set; }

        public decimal Valor { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? Observacoes { get; set; }
    }
}