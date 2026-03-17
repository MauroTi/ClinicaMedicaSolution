using ClinicaMedica.Consumidor.Models;
using ClinicaMedica.Consumidor.ViewModels;
using ClinicaMedica.Consumidor.Services;
using Microsoft.AspNetCore.Mvc;

public class ConsultasController : Controller
{
    private readonly ApiService _apiService;

    public ConsultasController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var consultas = await _apiService.GetAllAsync<Consulta>("api/consultas");
            var medicos = await _apiService.GetAllAsync<Medico>("api/medicosapi");
            var pacientes = await _apiService.GetAllAsync<Paciente>("api/pacientes");

            // DEBUG: veja quantos registros chegaram
            ViewData["TotalConsultas"] = consultas.Count;

            var model = consultas.Select(c => new ConsultaViewModel
            {
                Id = c.Id,
                MedicoId = c.MedicoId,
                PacienteId = c.PacienteId,
                DataHoraConsulta = c.DataHoraConsulta,
                Valor = c.Valor,
                Status = c.Status,
                Observacoes = c.Observacoes,
                DataCadastro = c.DataCadastro,
                NomeMedico = medicos.FirstOrDefault(m => m.Id == c.MedicoId)?.Nome ?? "-",
                NomePaciente = pacientes.FirstOrDefault(p => p.Id == c.PacienteId)?.Nome ?? "-"
            }).ToList();

            return View(model);
        }
        catch (Exception ex)
        {
            ViewData["ErroApi"] = $"Erro ao consumir a API: {ex.Message}";
            return View(new List<ConsultaViewModel>());
        }
    }
}