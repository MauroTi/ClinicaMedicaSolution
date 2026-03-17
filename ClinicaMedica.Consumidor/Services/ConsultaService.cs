// ========================= ConsultaService.cs =========================
using ClinicaMedica.Consumidor.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClinicaMedica.Consumidor.Services
{
    public class ConsultaService
    {
        private readonly ApiService _api;
        private const string _endpoint = "consultas";

        public ConsultaService(ApiService api)
        {
            _api = api;
        }

        public Task<List<Consulta>> ObterTodosAsync() => _api.GetAllAsync<Consulta>(_endpoint);
        public Task<Consulta?> ObterPorIdAsync(int id) => _api.GetByIdAsync<Consulta>(_endpoint, id);
        public Task<HttpResponseMessage> AdicionarAsync(Consulta consulta) => _api.PostAsync(_endpoint, consulta);
        public Task<HttpResponseMessage> AtualizarAsync(int id, Consulta consulta) => _api.PutAsync(_endpoint, id, consulta);
        public Task<HttpResponseMessage> ExcluirAsync(int id) => _api.DeleteAsync(_endpoint, id);
    }
}