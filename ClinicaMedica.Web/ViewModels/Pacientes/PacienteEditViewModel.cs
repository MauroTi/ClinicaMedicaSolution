using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.Web.ViewModels.Pacientes
{
    public class PacienteEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(14)]
        public string Cpf { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime? DataNascimento { get; set; }

        [Required]
        [StringLength(20)]
        public string Telefone { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        public bool Ativo { get; set; } = true;
    }
}