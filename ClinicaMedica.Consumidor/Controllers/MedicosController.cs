using Microsoft.AspNetCore.Mvc;
using ClinicaMedica.Consumidor.Services;
using ClinicaMedica.Consumidor.Models;
using ClinicaMedica.Consumidor.ViewModels;

namespace ClinicaMedica.Consumidor.Controllers
{
    public class MedicosController : Controller
    {
        private readonly MedicoService _service;
        public MedicosController(MedicoService service) => _service = service;

        public async Task<IActionResult> Index()
        {
            var medicos = await _service.ObterTodosAsync();
            var vm = medicos.Select(m => new MedicoViewModel
            {
                Id = m.Id,
                Nome = m.Nome,
                Especialidade = m.Especialidade,
                Crm = m.Crm,
                Telefone = m.Telefone,
                Email = m.Email,
                Ativo = m.Ativo
            }).ToList();
            return View(vm);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(MedicoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var medico = new Medico
            {
                Nome = model.Nome,
                Especialidade = model.Especialidade,
                Crm = model.Crm,
                Telefone = model.Telefone,
                Email = model.Email,
                Ativo = model.Ativo,
                DataCadastro = DateTime.Now
            };
            await _service.AdicionarAsync(medico);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var medico = await _service.ObterPorIdAsync(id);
            if (medico == null) return NotFound();
            var vm = new MedicoViewModel
            {
                Id = medico.Id,
                Nome = medico.Nome,
                Especialidade = medico.Especialidade,
                Crm = medico.Crm,
                Telefone = medico.Telefone,
                Email = medico.Email,
                Ativo = medico.Ativo
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MedicoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var medico = new Medico
            {
                Id = model.Id,
                Nome = model.Nome,
                Especialidade = model.Especialidade,
                Crm = model.Crm,
                Telefone = model.Telefone,
                Email = model.Email,
                Ativo = model.Ativo,
                DataCadastro = DateTime.Now
            };
            await _service.AtualizarAsync(model.Id, medico);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var medico = await _service.ObterPorIdAsync(id);
            if (medico == null) return NotFound();
            var vm = new MedicoViewModel
            {
                Id = medico.Id,
                Nome = medico.Nome,
                Especialidade = medico.Especialidade,
                Crm = medico.Crm,
                Telefone = medico.Telefone,
                Email = medico.Email,
                Ativo = medico.Ativo
            };
            return View(vm);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var medico = await _service.ObterPorIdAsync(id);
            if (medico == null) return NotFound();
            await _service.ExcluirAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}