using ClinicaMedica.Web.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ClinicaMedica.Web.Daos.Interfaces
{
    public interface IPacienteDao
    {
        Task CriarAsync(Paciente model);
        Task<bool> EditarAsync(Paciente model);
        Task<bool> ExcluirAsync(int id);
        IEnumerable<Paciente> ObterTodos();
        Paciente? ObterPorId(int id);
        Task<bool> ExisteCpfAsync(string cpf);
    }
}