using ClinicaMedica.Web.Models;

namespace ClinicaMedica.Web.Services.Interfaces
{
    public interface IPacienteService
    {
        Task<IEnumerable<Paciente>> ObterTodosAsync();
        Task<Paciente?> ObterPorIdAsync(int id);
        Task<int> AdicionarAsync(Paciente paciente);
        Task<bool> AtualizarAsync(Paciente paciente);
        Task<bool> ExcluirAsync(int id);
        Task<bool> ExisteCpfAsync(string cpf);
    }
}