namespace ClinicaMedica.Web.ViewModels.Medico
{
    public class MedicoDetailsViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Especialidade { get; set; } = string.Empty;
        public string Crm { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}