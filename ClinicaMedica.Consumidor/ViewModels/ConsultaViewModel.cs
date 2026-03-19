using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ClinicaMedica.Consumidor.ViewModels
{
    public class ConsultaViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Médico")]
        [Required(ErrorMessage = "Selecione o médico.")]
        public int MedicoId { get; set; }

        [Display(Name = "Paciente")]
        [Required(ErrorMessage = "Selecione o paciente.")]
        public int PacienteId { get; set; }

        [Display(Name = "Data e Hora da Consulta")]
        [Required(ErrorMessage = "Informe a data e hora da consulta.")]
        public DateTime DataHoraConsulta { get; set; }

        [Display(Name = "Valor")]
        [Required(ErrorMessage = "Informe o valor da consulta.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        public decimal Valor { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Informe o status da consulta.")]
        [StringLength(50, ErrorMessage = "O status deve ter no máximo 50 caracteres.")]
        public required string Status { get; set; }

        [Display(Name = "Observações")]
        [StringLength(500, ErrorMessage = "As observações devem ter no máximo 500 caracteres.")]
        public string? Observacoes { get; set; }

        [Display(Name = "Data de Cadastro")]
        public DateTime DataCadastro { get; set; }

        [JsonIgnore]
        public string? NomeMedico { get; set; }

        [JsonIgnore]
        public string? NomePaciente { get; set; }

        [JsonIgnore]
        public List<MedicoViewModel> Medicos { get; set; } = new();

        [JsonIgnore]
        public List<PacienteViewModel> Pacientes { get; set; } = new();
    }
}