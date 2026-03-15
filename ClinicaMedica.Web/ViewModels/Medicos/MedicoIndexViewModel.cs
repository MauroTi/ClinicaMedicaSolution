using ClinicaMedica.Web.Models;
using System.Collections.Generic;

namespace ClinicaMedica.Web.ViewModels.Medicos
{
    public class MedicoIndexViewModel
    {
        public IEnumerable<ClinicaMedica.Web.Models.Medico> Medicos { get; set; }
            = new List<ClinicaMedica.Web.Models.Medico>();
    }
}