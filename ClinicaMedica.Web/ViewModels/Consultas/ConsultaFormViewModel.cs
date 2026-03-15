using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClinicaMedica.Web.ViewModels.Consultas
{
    public class ConsultaFormViewModel
    {
        public int Id { get; set; }

        [Required] public int MedicoId { get; set; }
        [Required] public int PacienteId { get; set; }
        [Required] public DateTime DataHoraConsulta { get; set; }
        [Required] public decimal Valor { get; set; }
        [Required] public string Status { get; set; } = string.Empty;
        public string Observacoes { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }

        public string MedicoNome { get; set; } = string.Empty;
        public string PacienteNome { get; set; } = string.Empty;

        public IEnumerable<SelectListItem> Medicos { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Pacientes { get; set; } = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> StatusDisponiveis { get; set; } = new List<SelectListItem>();
        public bool ModoSomenteLeitura { get; set; } = false;
    }
}