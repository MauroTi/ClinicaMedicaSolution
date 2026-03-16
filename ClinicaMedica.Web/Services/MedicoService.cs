// Path: Services/MedicoService.cs
using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.Models;
using ClinicaMedica.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
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

        public async Task<bool> AdicionarAsync(Medico medico)
        {
            var existente = await _medicoDao.ObterPorCrmAsync(medico.Crm);
            if (existente != null)
                throw new Exception($"O CRM {medico.Crm} já está cadastrado.");

            return await _medicoDao.AdicionarAsync(medico);
        }

        public async Task<bool> AtualizarAsync(Medico medico)
        {
            var existente = await _medicoDao.ObterPorCrmAsync(medico.Crm);
            if (existente != null && existente.Id != medico.Id)
                throw new Exception($"O CRM {medico.Crm} já está cadastrado para outro médico.");

            return await _medicoDao.AtualizarAsync(medico);
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            return await _medicoDao.ExcluirAsync(id);
        }

        public async Task<Medico> ObterPorIdAsync(int id)
        {
            return await _medicoDao.ObterPorId(id);
        }

        public async Task<IEnumerable<Medico>> ObterTodosAsync()
        {
            return await _medicoDao.ObterTodos();
        }
    }
}