using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.Models;
using ClinicaMedica.Web.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicaMedica.Web.Services
{
    public class MedicoService : IMedicoService
    {
        private readonly IMedicoDao _medicoDao;

        public MedicoService(IMedicoDao medicoDao)
        {
            _medicoDao = medicoDao;
        }

        public Task<IEnumerable<Medico>> ObterTodosAsync()
        {
            var medicos = _medicoDao.ObterTodos();
            return Task.FromResult(medicos);
        }

        public Task<Medico?> ObterPorIdAsync(int id)
        {
            var medico = _medicoDao.ObterPorId(id);
            return Task.FromResult(medico);
        }

        public async Task<int> AdicionarAsync(Medico medico)
        {
            await _medicoDao.AdicionarAsync(medico);
            return medico.Id;
        }

        public async Task<bool> AtualizarAsync(Medico medico)
        {
            return await _medicoDao.AtualizarAsync(medico);
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            return await _medicoDao.ExcluirAsync(id);
        }
    }
}