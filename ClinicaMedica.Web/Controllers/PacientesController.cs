using ClinicaMedica.Web.Helpers;
using ClinicaMedica.Web.Models;
using ClinicaMedica.Web.Services.Interfaces;
using ClinicaMedica.Web.ViewModels.Pacientes;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedica.Web.Controllers;

public class PacientesController : Controller
{
    private const int PageSize = 10;
    private readonly IPacienteService _pacienteService;

    public PacientesController(IPacienteService pacienteService)
    {
        _pacienteService = pacienteService;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        var pacientes = (await _pacienteService.ObterTodosAsync()).ToList();
        var pagination = PaginationHelper.Create(page, pacientes.Count, PageSize);

        var viewModel = new PacienteIndexViewModel
        {
            Pacientes = PaginationHelper.Slice(pacientes, pagination.CurrentPage, PageSize),
            Pagination = pagination
        };

        return View(viewModel);
    }

    public IActionResult Create()
    {
        return View(new PacienteFormViewModel
        {
            DataNascimento = DateTime.Today,
            Ativo = true,
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PacienteFormViewModel vm)
    {
        if (string.IsNullOrWhiteSpace(vm.Cpf))
        {
            ModelState.AddModelError("Cpf", "O CPF é obrigatório.");
        }

        if (await _pacienteService.ExisteCpfAsync(vm.Cpf))
        {
            ModelState.AddModelError("Cpf", "Este CPF já está cadastrado.");
        }

        if (!ModelState.IsValid)
            return View(vm);

        var paciente = new Paciente
        {
            Nome = vm.Nome,
            Cpf = vm.Cpf,
            Telefone = vm.Telefone,
            Email = vm.Email,
            DataNascimento = vm.DataNascimento,
            Ativo = vm.Ativo,
            DataCadastro = DateTime.Now
        };

        await _pacienteService.AdicionarAsync(paciente);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var paciente = await _pacienteService.ObterPorIdAsync(id);
        if (paciente == null) return NotFound();

        var vm = new PacienteFormViewModel
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
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PacienteFormViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var paciente = await _pacienteService.ObterPorIdAsync(vm.Id);
        if (paciente == null) return NotFound();

        paciente.Nome = vm.Nome;
        paciente.Cpf = vm.Cpf;
        paciente.Telefone = vm.Telefone;
        paciente.Email = vm.Email;
        paciente.DataNascimento = vm.DataNascimento;
        paciente.Ativo = vm.Ativo;

        await _pacienteService.AtualizarAsync(paciente);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var paciente = await _pacienteService.ObterPorIdAsync(id);
        if (paciente == null) return NotFound();

        return View(paciente);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var paciente = await _pacienteService.ObterPorIdAsync(id);
        if (paciente == null) return NotFound();

        return View(paciente);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _pacienteService.ExcluirAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
