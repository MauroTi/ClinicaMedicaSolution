using ClinicaMedica.Web.Daos.Interfaces;
using ClinicaMedica.Web.Models;
using ClinicaMedica.Web.Services.Interfaces;

namespace ClinicaMedica.Web.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteDao _pacienteDao;

        public PacienteService(IPacienteDao pacienteDao)
        {
            _pacienteDao = pacienteDao;
        }

        // Obter todos os pacientes
        public Task<IEnumerable<Paciente>> ObterTodosAsync()
        {
            return Task.FromResult(_pacienteDao.ObterTodos());
        }

        // Obter paciente por ID
        public Task<Paciente?> ObterPorIdAsync(int id)
        {
            return Task.FromResult(_pacienteDao.ObterPorId(id));
        }

        // Criar paciente
        public async Task<int> AdicionarAsync(Paciente model)
        {
            await _pacienteDao.CriarAsync(model);
            return model.Id; // assumindo que o DAO seta o Id após criar
        }

        // Editar paciente
        public async Task<bool> AtualizarAsync(Paciente model)
        {
            return await _pacienteDao.EditarAsync(model);
        }

        // Excluir paciente
        public async Task<bool> ExcluirAsync(int id)
        {
            return await _pacienteDao.ExcluirAsync(id);
        }

        public async Task<bool> ExisteCpfAsync(string cpf)
        {
            return await _pacienteDao.ExisteCpfAsync(cpf);
        }
    }
}