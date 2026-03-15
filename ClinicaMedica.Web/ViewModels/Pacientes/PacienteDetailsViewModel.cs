namespace ClinicaMedica.Web.ViewModels.Paciente
{
    public class PacienteDetailsViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public DateTime? DataNascimento { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}