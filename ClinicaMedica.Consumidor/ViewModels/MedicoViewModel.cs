using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.Consumidor.ViewModels
{
    public class MedicoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [Display(Name = "Nome")]
        [StringLength(150, ErrorMessage = "O nome deve ter no máximo 150 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A especialidade é obrigatória.")]
        [Display(Name = "Especialidade")]
        [StringLength(100, ErrorMessage = "A especialidade deve ter no máximo 100 caracteres.")]
        public string Especialidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CRM é obrigatório.")]
        [Display(Name = "CRM")]
        [StringLength(20, ErrorMessage = "O CRM deve ter no máximo 20 caracteres.")]
        public string Crm { get; set; } = string.Empty;

        [Display(Name = "Telefone")]
        [StringLength(20, ErrorMessage = "O telefone deve ter no máximo 20 caracteres.")]
        public string? Telefone { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        [StringLength(150, ErrorMessage = "O e-mail deve ter no máximo 150 caracteres.")]
        public string? Email { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        [Display(Name = "Data de Cadastro")]
        public DateTime DataCadastro { get; set; }
    }
}