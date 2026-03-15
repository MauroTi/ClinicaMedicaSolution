using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.Models;

namespace ClinicaMedica.Web.Services
{
    public class PacienteService
    {
        private readonly IPacienteDao _pacienteDao;

        public PacienteService(IPacienteDao pacienteDao)
        {
            _pacienteDao = pacienteDao;
        }

        // Obter todos os pacientes
        public IEnumerable<Paciente> ObterTodos()
        {
            return _pacienteDao.ObterTodos();
        }

        // Obter paciente por ID
        public Paciente? ObterPorId(int id)
        {
            return _pacienteDao.ObterPorId(id);
        }

        // Excluir paciente (retorna o paciente excluído ou null)
        public async Task<bool> ExcluirAsync(int id)
        {
            return await _pacienteDao.ExcluirAsync(id);
        }

        // Criar paciente (agora PUBLIC e chamando o DAO real)
        public async Task CriarAsync(Paciente model)
        {
            await _pacienteDao.CriarAsync(model);
        }

        //Editar paciente
        public async Task<bool> EditarAsync(Paciente model)
        {
            return await _pacienteDao.EditarAsync(model);
        }

    }
}