using Microsoft.AspNetCore.Mvc;
using ClinicaMedica.Consumidor.Services;
using ClinicaMedica.Consumidor.Models;
using ClinicaMedica.Consumidor.ViewModels;

namespace ClinicaMedica.Consumidor.Controllers
{
    public class ConsultasController : Controller
    {
        private readonly ConsultaService _service;
        private readonly MedicoService _medicos;
        private readonly PacienteService _pacientes;

        public ConsultasController(ConsultaService service, MedicoService medicos, PacienteService pacientes)
        {
            _service = service;
            _medicos = medicos;
            _pacientes = pacientes;
        }

        public async Task<IActionResult> Index()
        {
            var consultas = await _service.ObterTodosAsync();
            var medicos = await _medicos.ObterTodosAsync();
            var pacientes = await _pacientes.ObterTodosAsync();

            var vm = consultas.Select(c => new ConsultaViewModel
            {
                Id = c.Id,
                MedicoId = c.MedicoId,
                PacienteId = c.PacienteId,
                DataHoraConsulta = c.DataHoraConsulta,
                Valor = c.Valor,
                Status = c.Status,
                Observacoes = c.Observacoes,
                NomeMedico = medicos.FirstOrDefault(m => m.Id == c.MedicoId)?.Nome,
                NomePaciente = pacientes.FirstOrDefault(p => p.Id == c.PacienteId)?.Nome
            }).ToList();
            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Medicos = await _medicos.ObterTodosAsync();
            ViewBag.Pacientes = await _pacientes.ObterTodosAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ConsultaViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var consulta = new Consulta
            {
                MedicoId = model.MedicoId,
                PacienteId = model.PacienteId,
                DataHoraConsulta = model.DataHoraConsulta,
                Valor = model.Valor,
                Status = model.Status,
                Observacoes = model.Observacoes,
                DataCadastro = DateTime.Now
            };
            await _service.AdicionarAsync(consulta);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var consulta = await _service.ObterPorIdAsync(id);
            if (consulta == null) return NotFound();
            var vm = new ConsultaViewModel
            {
                Id = consulta.Id,
                MedicoId = consulta.MedicoId,
                PacienteId = consulta.PacienteId,
                DataHoraConsulta = consulta.DataHoraConsulta,
                Valor = consulta.Valor,
                Status = consulta.Status,
                Observacoes = consulta.Observacoes
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ConsultaViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var consulta = new Consulta
            {
                Id = model.Id,
                MedicoId = model.MedicoId,
                PacienteId = model.PacienteId,
                DataHoraConsulta = model.DataHoraConsulta,
                Valor = model.Valor,
                Status = model.Status,
                Observacoes = model.Observacoes,
                DataCadastro = DateTime.Now
            };
            await _service.AtualizarAsync(model.Id, consulta);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var consulta = await _service.ObterPorIdAsync(id);
            if (consulta == null) return NotFound();
            var medico = await _medicos.ObterPorIdAsync(consulta.MedicoId);
            var paciente = await _pacientes.ObterPorIdAsync(consulta.PacienteId);
            var vm = new ConsultaViewModel
            {
                Id = consulta.Id,
                MedicoId = consulta.MedicoId,
                PacienteId = consulta.PacienteId,
                DataHoraConsulta = consulta.DataHoraConsulta,
                Valor = consulta.Valor,
                Status = consulta.Status,
                Observacoes = consulta.Observacoes,
                NomeMedico = medico?.Nome,
                NomePaciente = paciente?.Nome
            };
            return View(vm);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _service.ExcluirAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}