using ClinicaMedica.Consumidor.ViewModels;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClinicaMedica.Consumidor.Services
{
    public class PacienteService
    {
        private readonly ApiService _api;
        private const string _endpoint = "api/pacientes";

        public PacienteService(ApiService api)
        {
            _api = api;
        }

        public Task<List<PacienteViewModel>> ObterTodosAsync()
            => _api.GetAllAsync<PacienteViewModel>(_endpoint);

        public Task<PacienteViewModel?> ObterPorIdAsync(int id)
            => _api.GetByIdAsync<PacienteViewModel>(_endpoint, id);

        public Task<HttpResponseMessage> AdicionarAsync(PacienteViewModel paciente)
            => _api.PostAsync(_endpoint, paciente);

        public Task<HttpResponseMessage> AtualizarAsync(int id, PacienteViewModel paciente)
            => _api.PutAsync(_endpoint, id, paciente);

        public Task<HttpResponseMessage> ExcluirAsync(int id)
            => _api.DeleteAsync(_endpoint, id);
    }
}