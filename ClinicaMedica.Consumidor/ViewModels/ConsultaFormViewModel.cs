using ClinicaMedica.Consumidor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClinicaMedica.Consumidor.ViewModels
{
    public class ConsultaFormViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Data/Hora da Consulta")]
        public DateTime DataHoraConsulta { get; set; }

        [Display(Name = "Data de Cadastro")]
        public DateTime DataCadastro { get; set; }

        public int MedicoId { get; set; }
        public int PacienteId { get; set; }

        [Display(Name = "Valor")]
        public decimal Valor { get; set; }

        [Display(Name = "Observações")]
        public string Observacoes { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        public List<Medico> MedicosDisponiveis { get; set; }
        public List<Paciente> PacientesDisponiveis { get; set; }
    }
}