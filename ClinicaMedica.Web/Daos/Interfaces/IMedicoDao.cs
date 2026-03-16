// Path: Daos/Interfaces/IMedicoDao.cs
using ClinicaMedica.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicaMedica.Web.Daos.Interfaces
{
    public interface IMedicoDao
    {
        Task<bool> AdicionarAsync(Medico medico);
        Task<bool> AtualizarAsync(Medico medico);
        Task<bool> ExcluirAsync(int id);
        Task<Medico> ObterPorId(int id);
        Task<IEnumerable<Medico>> ObterTodos();
        Task<Medico> ObterPorCrmAsync(string crm); // para validação de CRM duplicado
    }
}