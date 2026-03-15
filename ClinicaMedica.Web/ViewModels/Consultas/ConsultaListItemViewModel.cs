namespace ClinicaMedica.Web.ViewModels.Consultas
{
    public class ConsultaListItemViewModel
    {
        public int Id { get; set; }
        public string PacienteNome { get; set; } = string.Empty;
        public string MedicoNome { get; set; } = string.Empty;
        public DateTime DataConsulta { get; set; }
        public string Observacoes { get; set; } = string.Empty;
    }
}