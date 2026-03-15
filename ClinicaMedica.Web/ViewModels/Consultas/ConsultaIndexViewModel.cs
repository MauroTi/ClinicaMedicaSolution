namespace ClinicaMedica.Web.ViewModels.Consultas
{
    public class ConsultaIndexViewModel
    {
        public int Id { get; set; }
        public string MedicoNome { get; set; } = string.Empty;
        public string PacienteNome { get; set; } = string.Empty;
        public DateTime DataHoraConsulta { get; set; }
        public decimal Valor { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Observacoes { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }
        public IEnumerable<ConsultaFormViewModel> Consultas { get; set; } = new List<ConsultaFormViewModel>();
    }
}