namespace ClinicaMedica.Consumidor.ViewModels
{
    public class ConsultaViewModel
    {
        public int Id { get; set; }
        public int MedicoId { get; set; }
        public int PacienteId { get; set; }
        public DateTime DataHoraConsulta { get; set; }
        public decimal Valor { get; set; }
        public required string Status { get; set; }
        public string? Observacoes { get; set; }
        public DateTime DataCadastro { get; set; }
        public required string NomeMedico { get; set; }
        public required string NomePaciente { get; set; }

    }
}