namespace ClinicaMedica.Web.Core.Entities
{
    public class Medico : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string Crm { get; set; } = string.Empty;
        public string Especialidade { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
    }
}