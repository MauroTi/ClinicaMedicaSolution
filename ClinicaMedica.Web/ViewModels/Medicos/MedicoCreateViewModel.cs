using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.Web.ViewModels.Medicos
{
    public class MedicoCreateViewModel
    {
        [Required]
        [StringLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Especialidade { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Crm { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Telefone { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        public bool Ativo { get; set; } = true;
    }
}