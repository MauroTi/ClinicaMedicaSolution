using ClinicaMedica.Consumidor.ViewModels;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClinicaMedica.Consumidor.Services
{
    public class MedicoService
    {
        private readonly ApiService _api;
        private const string _endpoint = "api/medicosApi";

        public MedicoService(ApiService api)
        {
            _api = api;
        }

        public Task<List<MedicoViewModel>> ObterTodosAsync()
            => _api.GetAllAsync<MedicoViewModel>(_endpoint);

        public Task<MedicoViewModel?> ObterPorIdAsync(int id)
            => _api.GetByIdAsync<MedicoViewModel>(_endpoint, id);

        public Task<HttpResponseMessage> AdicionarAsync(MedicoViewModel medico)
            => _api.PostAsync(_endpoint, medico);

        public Task<HttpResponseMessage> AtualizarAsync(int id, MedicoViewModel medico)
            => _api.PutAsync(_endpoint, id, medico);

        public Task<HttpResponseMessage> ExcluirAsync(int id)
            => _api.DeleteAsync(_endpoint, id);
    }
}