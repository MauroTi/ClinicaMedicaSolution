namespace ClinicaMedica.Web.Application.Dtos
{
    public class ConsultaDto
    {
        public int Id { get; set; }
        public int MedicoId { get; set; }
        public int PacienteId { get; set; }
        public DateTime DataConsulta { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public string Diagnostico { get; set; } = string.Empty;
        public string Prescricao { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }
    }

    public class CreateConsultaDto
    {
        public int MedicoId { get; set; }
        public int PacienteId { get; set; }
        public DateTime DataConsulta { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }
}