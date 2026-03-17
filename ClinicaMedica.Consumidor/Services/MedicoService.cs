// ========================= MedicoService.cs =========================
using ClinicaMedica.Consumidor.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClinicaMedica.Consumidor.Services
{
    public class MedicoService
    {
        private readonly ApiService _api;
        private const string _endpoint = "medicos";

        public MedicoService(ApiService api)
        {
            _api = api;
        }

        public Task<List<Medico>> ObterTodosAsync() => _api.GetAllAsync<Medico>(_endpoint);
        public Task<Medico?> ObterPorIdAsync(int id) => _api.GetByIdAsync<Medico>(_endpoint, id);
        public Task<HttpResponseMessage> AdicionarAsync(Medico medico) => _api.PostAsync(_endpoint, medico);
        public Task<HttpResponseMessage> AtualizarAsync(int id, Medico medico) => _api.PutAsync(_endpoint, id, medico);
        public Task<HttpResponseMessage> ExcluirAsync(int id) => _api.DeleteAsync(_endpoint, id);
    }
}