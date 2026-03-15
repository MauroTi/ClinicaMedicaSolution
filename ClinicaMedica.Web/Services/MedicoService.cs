using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.Models;

namespace ClinicaMedica.Web.Services
{
    public class MedicoService
    {
        private readonly IMedicoDao _medicoDao;

        public MedicoService(IMedicoDao medicoDao)
        {
            _medicoDao = medicoDao;
        }

        public IEnumerable<Medico> ObterTodos()
        {
            return _medicoDao.ObterTodos();
        }

        public Medico? ObterPorId(int id)
        {
            return _medicoDao.ObterPorId(id);
        }
    }
}