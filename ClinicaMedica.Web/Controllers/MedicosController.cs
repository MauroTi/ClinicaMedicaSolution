using ClinicaMedica.Web.Helpers;
using ClinicaMedica.Web.Models;
using ClinicaMedica.Web.Services.Interfaces;
using ClinicaMedica.Web.ViewModels.Medicos;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedica.Web.Controllers;

public class MedicosController : Controller
{
    private const int PageSize = 10;
    private readonly IMedicoService _medicoService;

    public MedicosController(IMedicoService medicoService)
    {
        _medicoService = medicoService;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        var medicos = await _medicoService.ObterTodosAsync();
        var lista = medicos.ToList();
        var pagination = PaginationHelper.Create(page, lista.Count, PageSize);

        var vm = new MedicoIndexViewModel
        {
            Medicos = PaginationHelper.Slice(lista, pagination.CurrentPage, PageSize),
            Pagination = pagination
        };

        return View(vm);
    }

    public IActionResult Create() => View(new MedicoFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MedicoFormViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var medico = new Medico
        {
            Nome = vm.Nome,
            Crm = vm.Crm,
            Especialidade = vm.Especialidade,
            Telefone = vm.Telefone,
            Email = vm.Email,
            Ativo = vm.Ativo,
            DataCadastro = DateTime.Now
        };

        try
        {
            await _medicoService.AdicionarAsync(medico);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(vm);
        }

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
            Crm = medico.Crm,
            Especialidade = medico.Especialidade,
            Telefone = medico.Telefone,
            Email = medico.Email,
            Ativo = medico.Ativo,
            DataCadastro = medico.DataCadastro
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(MedicoFormViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

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

        try
        {
            var sucesso = await _medicoService.AtualizarAsync(medico);
            if (!sucesso) ModelState.AddModelError("", "Erro ao atualizar o médico.");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(vm);
        }

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
        bool sucesso = await _medicoService.ExcluirAsync(id);
        if (!sucesso) return NotFound();
        return RedirectToAction(nameof(Index));
    }
}
