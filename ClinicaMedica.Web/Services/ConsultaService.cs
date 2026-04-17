using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.DTOs;
using ClinicaMedica.Web.Models;
using ClinicaMedica.Web.Services.Interfaces;

namespace ClinicaMedica.Web.Services
{
    public class ConsultaService : IConsultaService
    {
        private readonly IConsultaDao _consultaDao;

        public ConsultaService(IConsultaDao consultaDao)
        {
            _consultaDao = consultaDao;
        }

        public async Task<IEnumerable<Consulta>> ObterTodosAsync()
        {
            return await _consultaDao.ObterTodosAsync();
        }

        public async Task<IEnumerable<ConsultaDto>> ObterTodosDetalhadosAsync()
        {
            return await _consultaDao.ObterTodosDetalhadosAsync();
        }

        public async Task<IDictionary<string, int>> ObterStatusAgrupadoAsync()
        {
            var consultas = await _consultaDao.ObterTodosAsync();

            return consultas
                .GroupBy(c => string.IsNullOrWhiteSpace(c.Status) ? "Sem status" : c.Status.Trim())
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task<Consulta?> ObterPorIdAsync(int id)
        {
            return await _consultaDao.ObterPorIdAsync(id);
        }

        public async Task<int> InserirAsync(Consulta consulta)
        {
            return await _consultaDao.InserirAsync(consulta);
        }

        public async Task<bool> AtualizarAsync(Consulta consulta)
        {
            return await _consultaDao.AtualizarAsync(consulta);
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            return await _consultaDao.ExcluirAsync(id);
        }
    }
}
