using ClinicaMedica.Web.Models;

namespace ClinicaMedica.Web.Services.Interfaces
{
    public interface IMedicoService
    {
        Task<bool> AdicionarAsync(Medico medico);
        Task<bool> AtualizarAsync(Medico medico);
        Task<bool> ExcluirAsync(int id);
        Task<Medico?> ObterPorIdAsync(int id);
        Task<IEnumerable<Medico>> ObterTodosAsync();
    }
}
