using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClinicaMedica.Web.ViewModels.Consultas
{
    public class ConsultaFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Selecione o médico.")]
        [Display(Name = "Médico")]
        public int MedicoId { get; set; }

        [Required(ErrorMessage = "Selecione o paciente.")]
        [Display(Name = "Paciente")]
        public int PacienteId { get; set; }

        [Required(ErrorMessage = "Informe a data e hora da consulta.")]
        [Display(Name = "Data/Hora da Consulta")]
        public DateTime DataHoraConsulta { get; set; }

        [Required(ErrorMessage = "Informe o valor da consulta.")]
        [Display(Name = "Valor")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "Selecione o status da consulta.")]
        [Display(Name = "Status")]
        public string Status { get; set; } = string.Empty;

        [Display(Name = "Observações")]
        public string Observacoes { get; set; } = string.Empty;

        public DateTime DataCadastro { get; set; }

        public string MedicoNome { get; set; } = string.Empty;
        public string PacienteNome { get; set; } = string.Empty;

        public IEnumerable<SelectListItem> Medicos { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Pacientes { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> StatusDisponiveis { get; set; } = Enumerable.Empty<SelectListItem>();

        public bool ModoSomenteLeitura { get; set; } = false;
    }
}