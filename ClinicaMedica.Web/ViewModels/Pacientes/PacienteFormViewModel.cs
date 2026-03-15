using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.Web.ViewModels.Pacientes
{
    public class PacienteFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(150)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(14)]
        public string Cpf { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        public string Telefone { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        public bool Ativo { get; set; } = true;
        public DateTime DataCadastro { get; set; }
        public bool ModoSomenteLeitura { get; set; } = false;
    }
}