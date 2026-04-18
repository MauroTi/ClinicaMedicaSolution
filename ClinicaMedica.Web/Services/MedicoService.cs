using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.Infrastructure.Database;
using ClinicaMedica.Web.Models;
using ClinicaMedica.Web.Services.Interfaces;

namespace ClinicaMedica.Web.Services
{
    public class MedicoService : IMedicoService
    {
        private readonly IMedicoDao _medicoDao;

        public MedicoService(IMedicoDao medicoDao)
        {
            _medicoDao = medicoDao;
        }

        public async Task<bool> AdicionarAsync(Medico medico)
        {
            if (medico.DataCadastro == default)
                medico.DataCadastro = DateTime.Now;

            var existente = await _medicoDao.ObterPorCrmAsync(medico.Crm);
            if (existente is not null)
                throw new InvalidOperationException($"O CRM {medico.Crm} já está cadastrado.");

            var adicionado = await _medicoDao.AdicionarAsync(medico);

            if (adicionado)
            {
                var persistido = await _medicoDao.ObterPorCrmAsync(medico.Crm);
                medico.Id = persistido?.Id ?? medico.Id;
            }

            return adicionado;
        }

        public async Task<bool> AtualizarAsync(Medico medico)
        {
            var existente = await _medicoDao.ObterPorCrmAsync(medico.Crm);
            if (existente is not null && existente.Id != medico.Id)
                throw new InvalidOperationException($"O CRM {medico.Crm} já está cadastrado para outro médico.");

            return await _medicoDao.AtualizarAsync(medico);
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            try
            {
                return await _medicoDao.ExcluirAsync(id);
            }
            catch (Exception ex) when (DatabaseExceptionTranslator.IsReferenceConstraintViolation(ex))
            {
                throw new InvalidOperationException("Não é possível excluir o médico porque existem consultas vinculadas a ele.");
            }
        }

        public Task<Medico?> ObterPorIdAsync(int id) => _medicoDao.ObterPorId(id);

        public Task<IEnumerable<Medico>> ObterTodosAsync() => _medicoDao.ObterTodos();
    }
}
