using ClinicaMedica.Web.Models;
using ClinicaMedica.Web.Models.DTOs;

namespace ClinicaMedica.Web.Interfaces
{
    public interface IConsultaDao
    {
        Task<IEnumerable<Consulta>> ObterTodosAsync();
        Task<IEnumerable<ConsultaDto>> ObterTodosDetalhadosAsync();
        Task<Consulta?> ObterPorIdAsync(int id);
        Task<int> InserirAsync(Consulta consulta);
        Task<bool> AtualizarAsync(Consulta consulta);
        Task<bool> ExcluirAsync(int id);
        IEnumerable<Consulta> ObterTodos();
    }
}