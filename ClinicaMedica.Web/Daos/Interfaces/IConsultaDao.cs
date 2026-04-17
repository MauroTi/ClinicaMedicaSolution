using ClinicaMedica.Web.DTOs;
using ClinicaMedica.Web.Models;

namespace ClinicaMedica.Web.Daos.Interfaces;

public interface IConsultaDao
{
    Task<IEnumerable<Consulta>> ObterTodosAsync();
    Task<Consulta?> ObterPorIdAsync(int id);

    Task<IEnumerable<ConsultaDto>> ObterTodosDetalhadosAsync();

    Task<int> InserirAsync(Consulta consulta);
    Task<bool> AtualizarAsync(Consulta consulta);
    Task<bool> ExcluirAsync(int id);
}