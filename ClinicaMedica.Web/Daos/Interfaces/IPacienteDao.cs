using ClinicaMedica.Web.Models;

namespace ClinicaMedica.Web.Daos.Interfaces;

public interface IPacienteDao
{
    Task<IEnumerable<Paciente>> ObterTodos();
    Task<Paciente?> ObterPorId(int id);

    Task CriarAsync(Paciente model);
    Task<bool> EditarAsync(Paciente model);
    Task<bool> ExcluirAsync(int id);

    Task<bool> ExisteCpfAsync(string cpf);
}