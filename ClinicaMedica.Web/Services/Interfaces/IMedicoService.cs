using ClinicaMedica.Web.Models;

namespace ClinicaMedica.Web.Services.Interfaces
{
    public interface IMedicoService
    {
        Task<IEnumerable<Medico>> ObterTodosAsync();
        Task<Medico?> ObterPorIdAsync(int id);
        Task<int> AdicionarAsync(Medico medico);
        Task<bool> AtualizarAsync(Medico medico);
        Task<bool> ExcluirAsync(int id);
    }
}