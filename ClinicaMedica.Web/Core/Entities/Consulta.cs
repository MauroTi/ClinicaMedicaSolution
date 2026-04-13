namespace ClinicaMedica.Web.Core.Entities
{
    public class Consulta : BaseEntity
    {
        public int MedicoId { get; set; }
        public int PacienteId { get; set; }
        public DateTime DataConsulta { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public string Diagnostico { get; set; } = string.Empty;
        public string Prescricao { get; set; } = string.Empty;
        public string Status { get; set; } = "Agendada";

        // Construtor padrão
        public Consulta() { }

        // Construtor com parâmetros
        public Consulta(int medicoId, int pacienteId, DateTime dataConsulta, string motivo)
        {
            MedicoId = medicoId;
            PacienteId = pacienteId;
            DataConsulta = dataConsulta;
            Motivo = motivo;
            Status = "Agendada";
        }

        public void Atualizar(DateTime dataConsulta, string motivo, string diagnostico, string prescricao, string status)
        {
            DataConsulta = dataConsulta;
            Motivo = motivo;
            Diagnostico = diagnostico;
            Prescricao = prescricao;
            Status = status;
        }
    }
}