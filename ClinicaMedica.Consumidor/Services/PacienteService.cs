// ========================= PacienteService.cs =========================
using ClinicaMedica.Consumidor.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClinicaMedica.Consumidor.Services
{
    public class PacienteService
    {
        private readonly ApiService _api;
        private const string _endpoint = "pacientes";

        public PacienteService(ApiService api)
        {
            _api = api;
        }

        public Task<List<Paciente>> ObterTodosAsync() => _api.GetAllAsync<Paciente>(_endpoint);
        public Task<Paciente?> ObterPorIdAsync(int id) => _api.GetByIdAsync<Paciente>(_endpoint, id);
        public Task<HttpResponseMessage> AdicionarAsync(Paciente paciente) => _api.PostAsync(_endpoint, paciente);
        public Task<HttpResponseMessage> AtualizarAsync(int id, Paciente paciente) => _api.PutAsync(_endpoint, id, paciente);
        public Task<HttpResponseMessage> ExcluirAsync(int id) => _api.DeleteAsync(_endpoint, id);
    }
}