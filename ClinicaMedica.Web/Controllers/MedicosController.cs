using ClinicaMedica.Web.Models;
using ClinicaMedica.Web.Services.Interfaces;
using ClinicaMedica.Web.ViewModels.Medicos;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicaMedica.Web.Controllers
{
    public class MedicosController : Controller
    {
        private readonly IMedicoService _medicoService;

        public MedicosController(IMedicoService medicoService)
        {
            _medicoService = medicoService;
        }

        // GET: /Medicos
        public async Task<IActionResult> Index()
        {
            var medicos = await _medicoService.ObterTodosAsync();

            var viewModel = new MedicoIndexViewModel
            {
                Medicos = medicos.ToList()
            };

            return View(viewModel);
        }

        // GET: /Medicos/Create
        public IActionResult Create()
        {
            return View(new MedicoFormViewModel());
        }

        // POST: /Medicos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MedicoFormViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var medico = new Medico
            {
                Nome = vm.Nome,
                Crm = vm.Crm,
                Especialidade = vm.Especialidade,
                Telefone = vm.Telefone,
                Email = vm.Email,
                Ativo = vm.Ativo,
                DataCadastro = System.DateTime.Now
            };

            await _medicoService.AdicionarAsync(medico);

            return RedirectToAction(nameof(Index));
        }

        // GET: /Medicos/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var medico = await _medicoService.ObterPorIdAsync(id);
            if (medico == null)
                return NotFound();

            var vm = new MedicoFormViewModel
            {
                Id = medico.Id,
                Nome = medico.Nome,
                Crm = medico.Crm,
                Especialidade = medico.Especialidade,
                Telefone = medico.Telefone,
                Email = medico.Email,
                Ativo = medico.Ativo,
                DataCadastro = medico.DataCadastro
            };

            return View(vm);
        }

        // POST: /Medicos/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MedicoFormViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var medico = new Medico
            {
                Id = vm.Id,
                Nome = vm.Nome,
                Crm = vm.Crm,
                Especialidade = vm.Especialidade,
                Telefone = vm.Telefone,
                Email = vm.Email,
                Ativo = vm.Ativo,
                DataCadastro = vm.DataCadastro
            };

            bool sucesso = await _medicoService.AtualizarAsync(medico);
            if (!sucesso)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Medicos/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var medico = await _medicoService.ObterPorIdAsync(id);
            if (medico == null)
                return NotFound();

            return View(medico);
        }

        // GET: /Medicos/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var medico = await _medicoService.ObterPorIdAsync(id);
            if (medico == null)
                return NotFound();

            return View(medico);
        }

        // POST: /Medicos/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool sucesso = await _medicoService.ExcluirAsync(id);
            if (!sucesso)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}