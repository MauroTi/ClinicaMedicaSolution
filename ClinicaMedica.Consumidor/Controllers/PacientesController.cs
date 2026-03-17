using Microsoft.AspNetCore.Mvc;
using ClinicaMedica.Consumidor.Services;
using ClinicaMedica.Consumidor.Models;
using ClinicaMedica.Consumidor.ViewModels;

namespace ClinicaMedica.Consumidor.Controllers
{
    public class PacientesController : Controller
    {
        private readonly PacienteService _service;
        public PacientesController(PacienteService service) => _service = service;

        public async Task<IActionResult> Index()
        {
            var pacientes = await _service.ObterTodosAsync();
            var vm = pacientes.Select(p => new PacienteViewModel
            {
                Id = p.Id,
                Nome = p.Nome,
                Cpf = p.Cpf,
                Telefone = p.Telefone,
                Email = p.Email,
                DataNascimento = p.DataNascimento,
                Ativo = p.Ativo
            }).ToList();
            return View(vm);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(PacienteViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var paciente = new Paciente
            {
                Nome = model.Nome,
                Cpf = model.Cpf,
                Telefone = model.Telefone,
                Email = model.Email,
                DataNascimento = model.DataNascimento,
                Ativo = model.Ativo,
                DataCadastro = DateTime.Now
            };
            await _service.AdicionarAsync(paciente);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var paciente = await _service.ObterPorIdAsync(id);
            if (paciente == null) return NotFound();
            var vm = new PacienteViewModel
            {
                Id = paciente.Id,
                Nome = paciente.Nome,
                Cpf = paciente.Cpf,
                Telefone = paciente.Telefone,
                Email = paciente.Email,
                DataNascimento = paciente.DataNascimento,
                Ativo = paciente.Ativo
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PacienteViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var paciente = new Paciente
            {
                Id = model.Id,
                Nome = model.Nome,
                Cpf = model.Cpf,
                Telefone = model.Telefone,
                Email = model.Email,
                DataNascimento = model.DataNascimento,
                Ativo = model.Ativo,
                DataCadastro = DateTime.Now
            };
            await _service.AtualizarAsync(model.Id, paciente);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var paciente = await _service.ObterPorIdAsync(id);
            if (paciente == null) return NotFound();
            var vm = new PacienteViewModel
            {
                Id = paciente.Id,
                Nome = paciente.Nome,
                Cpf = paciente.Cpf,
                Telefone = paciente.Telefone,
                Email = paciente.Email,
                DataNascimento = paciente.DataNascimento,
                Ativo = paciente.Ativo
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