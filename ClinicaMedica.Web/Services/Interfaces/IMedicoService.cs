// Path: Services/Interfaces/IMedicoService.cs
using ClinicaMedica.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicaMedica.Web.Services.Interfaces
{
    public interface IMedicoService
    {
        Task<bool> AdicionarAsync(Medico medico);
        Task<bool> AtualizarAsync(Medico medico);
        Task<bool> ExcluirAsync(int id);
        Task<Medico> ObterPorIdAsync(int id);
        Task<IEnumerable<Medico>> ObterTodosAsync();
    }
}