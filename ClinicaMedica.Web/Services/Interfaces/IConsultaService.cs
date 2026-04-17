using ClinicaMedica.Web.DTOs;
using ClinicaMedica.Web.Models;

namespace ClinicaMedica.Web.Services.Interfaces;

public interface IConsultaService
{
    Task<IEnumerable<Consulta>> ObterTodosAsync();
    Task<IEnumerable<ConsultaDto>> ObterTodosDetalhadosAsync();
    Task<IDictionary<string, int>> ObterStatusAgrupadoAsync();
    Task<Consulta?> ObterPorIdAsync(int id);
    Task<int> InserirAsync(Consulta consulta);
    Task<bool> AtualizarAsync(Consulta consulta);
    Task<bool> ExcluirAsync(int id);
}
