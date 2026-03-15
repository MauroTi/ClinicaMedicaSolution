using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.Web.ViewModels.Medicos
{
    public class MedicoFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A especialidade é obrigatória.")]
        [StringLength(100)]
        public string Especialidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CRM é obrigatório.")]
        [StringLength(20)]
        public string Crm { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        public string Telefone { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        public bool Ativo { get; set; } = true;

        public DateTime DataCadastro { get; set; }
    }
}