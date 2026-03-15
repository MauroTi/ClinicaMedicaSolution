namespace ClinicaMedica.Web.Models
{
    public class Consulta
    {
        public int Id { get; set; }
        public int MedicoId { get; set; }
        public int PacienteId { get; set; }
        public DateTime DataHoraConsulta { get; set; }
        public decimal Valor { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Observacoes { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }
    }
}