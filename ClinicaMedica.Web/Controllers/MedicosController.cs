using ClinicaMedica.Web.Models;
using ClinicaMedica.Web.Services.Interfaces;
using ClinicaMedica.Web.ViewModels.Medicos;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedica.Web.Controllers
{
    public class MedicosController : Controller
    {
        private readonly IMedicoService _medicoService;

        public MedicosController(IMedicoService medicoService)
        {
            _medicoService = medicoService;
        }

        public async Task<IActionResult> Index()
        {
            var medicos = await _medicoService.ObterTodosAsync();
            return View(medicos);
        }

        public IActionResult Create()
        {
            return View(new MedicoFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MedicoFormViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var medico = new Medico
            {
                Nome = vm.Nome,
                Especialidade = vm.Especialidade,
                Crm = vm.Crm,
                Telefone = vm.Telefone,
                Email = vm.Email,
                Ativo = vm.Ativo,
                DataCadastro = DateTime.Now
            };

            await _medicoService.AdicionarAsync(medico);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var medico = await _medicoService.ObterPorIdAsync(id);
            if (medico == null) return NotFound();

            var vm = new MedicoFormViewModel
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MedicoFormViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var medico = await _medicoService.ObterPorIdAsync(vm.Id);
            if (medico == null) return NotFound();

            medico.Nome = vm.Nome;
            medico.Especialidade = vm.Especialidade;
            medico.Crm = vm.Crm;
            medico.Telefone = vm.Telefone;
            medico.Email = vm.Email;
            medico.Ativo = vm.Ativo;

            await _medicoService.AtualizarAsync(medico);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var medico = await _medicoService.ObterPorIdAsync(id);
            if (medico == null) return NotFound();

            return View(medico);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var medico = await _medicoService.ObterPorIdAsync(id);
            if (medico == null) return NotFound();

            return View(medico);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _medicoService.ExcluirAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}