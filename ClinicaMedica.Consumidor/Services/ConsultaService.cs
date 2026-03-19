using ClinicaMedica.Consumidor.ViewModels;

namespace ClinicaMedica.Consumidor.Services
{
    public class ConsultaService
    {
        private readonly ApiService _api;
        private const string _endpoint = "api/consultas";
        private const string _endpointMedicos = "api/medicosApi";
        private const string _endpointPacientes = "api/pacientes";

        public ConsultaService(ApiService api)
        {
            _api = api;
        }

        public async Task<List<ConsultaViewModel>> ObterTodosAsync()
        {
            var consultas = await _api.GetAllAsync<ConsultaViewModel>(_endpoint);
            await PreencherNomesAsync(consultas);
            return consultas;
        }

        public async Task<ConsultaViewModel?> ObterPorIdAsync(int id)
        {
            var consulta = await _api.GetByIdAsync<ConsultaViewModel>(_endpoint, id);

            if (consulta != null)
            {
                await PreencherNomesAsync(new List<ConsultaViewModel> { consulta });
            }

            return consulta;
        }

        public async Task<bool> CriarAsync(ConsultaViewModel consulta)
        {
            consulta.Medicos = new List<MedicoViewModel>();
            consulta.Pacientes = new List<PacienteViewModel>();

            var response = await _api.PostAsync(_endpoint, consulta);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AtualizarAsync(int id, ConsultaViewModel consulta)
        {
            consulta.Medicos = new List<MedicoViewModel>();
            consulta.Pacientes = new List<PacienteViewModel>();

            var response = await _api.PutAsync(_endpoint, id, consulta);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            var response = await _api.DeleteAsync(_endpoint, id);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<MedicoViewModel>> ObterMedicosAsync()
        {
            try
            {
                return await _api.GetAllAsync<MedicoViewModel>(_endpointMedicos);
            }
            catch
            {
                return new List<MedicoViewModel>();
            }
        }

        public async Task<List<PacienteViewModel>> ObterPacientesAsync()
        {
            try
            {
                return await _api.GetAllAsync<PacienteViewModel>(_endpointPacientes);
            }
            catch
            {
                return new List<PacienteViewModel>();
            }
        }

        public async Task PreencherListasAsync(ConsultaViewModel model)
        {
            model.Medicos = await ObterMedicosAsync();
            model.Pacientes = await ObterPacientesAsync();
        }

        private async Task PreencherNomesAsync(List<ConsultaViewModel> consultas)
        {
            if (consultas == null || !consultas.Any())
                return;

            var medicos = await ObterMedicosAsync();
            var pacientes = await ObterPacientesAsync();

            foreach (var c in consultas)
            {
                c.NomeMedico = medicos.FirstOrDefault(m => m.Id == c.MedicoId)?.Nome ?? $"ID {c.MedicoId}";
                c.NomePaciente = pacientes.FirstOrDefault(p => p.Id == c.PacienteId)?.Nome ?? $"ID {c.PacienteId}";
            }
        }

        public async Task<Dictionary<string, int>> ObterGraficoStatusAsync()
        {
            var consultas = await _api.GetAllAsync<ConsultaViewModel>(_endpoint);

            var grafico = consultas
                .GroupBy(c => string.IsNullOrWhiteSpace(c.Status) ? "Não informado" : c.Status.Trim())
                .ToDictionary(g => g.Key, g => g.Count());

            return grafico;
        }
    }
}