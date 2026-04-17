using ClinicaMedica.Web.Models;

namespace ClinicaMedica.Web.Daos.Interfaces;

public interface IMedicoDao
{
    Task<IEnumerable<Medico>> ObterTodos();
    Task<Medico?> ObterPorId(int id);
    Task<Medico?> ObterPorCrmAsync(string crm);

    Task<bool> AdicionarAsync(Medico medico);
    Task<bool> AtualizarAsync(Medico medico);
    Task<bool> ExcluirAsync(int id);
}