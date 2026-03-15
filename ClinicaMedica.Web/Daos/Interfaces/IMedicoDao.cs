using ClinicaMedica.Web.Models;

namespace ClinicaMedica.Web.Daos.Interfaces
{
    public interface IMedicoDao
    {
        IEnumerable<Medico> ObterTodos();
        Medico? ObterPorId(int id);
        Task AdicionarAsync(Medico medico);
        Task<bool> AtualizarAsync(Medico medico);
        Task<bool> ExcluirAsync(int id);
    }
}