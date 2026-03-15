namespace ClinicaMedica.Web.ViewModels.Consulta
{
    public class ConsultaListViewModel
    {
        public int Id { get; set; }
        public int MedicoId { get; set; }
        public string MedicoNome { get; set; } = string.Empty;
        public int PacienteId { get; set; }
        public string PacienteNome { get; set; } = string.Empty;
        public DateTime DataHoraConsulta { get; set; }
        public decimal Valor { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Observacoes { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}