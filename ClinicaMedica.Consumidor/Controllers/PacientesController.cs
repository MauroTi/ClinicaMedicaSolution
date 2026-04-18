using ClinicaMedica.Consumidor.Helpers;
using ClinicaMedica.Consumidor.Services.Interfaces;
using ClinicaMedica.Consumidor.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedica.Consumidor.Controllers;

public class PacientesController : ConsumerControllerBase
{
    private const int PageSize = 10;
    private readonly IPacienteService _service;

    public PacientesController(IPacienteService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        try
        {
            var pacientes = await _service.ObterTodosAsync();
            var pagination = CreatePagination(page, pacientes.Count, PageSize);

            var model = new PacienteIndexViewModel
            {
                Pacientes = PaginationHelper.Slice(pacientes, pagination.CurrentPage, PageSize),
                Pagination = pagination
            };

            return View(model);
        }
        catch (Exception ex)
        {
            ViewData["ErroApi"] = $"Erro ao carregar pacientes: {ex.Message}";
            return View(new PacienteIndexViewModel
            {
                Pagination = CreatePagination(1, 0, PageSize)
            });
        }
    }

    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var paciente = await _service.ObterPorIdAsync(id);

            if (paciente == null)
                return RedirectToIndexWithError("Paciente não encontrado.");

            return View(paciente);
        }
        catch (Exception ex)
        {
            return RedirectToIndexWithError($"Erro ao buscar paciente: {ex.Message}");
        }
    }

    public IActionResult Create()
    {
        return View(new PacienteViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PacienteViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var sucesso = await _service.CriarAsync(model);

            if (sucesso)
            {
                TempData["Sucesso"] = "Paciente cadastrado com sucesso.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Erro ao cadastrar paciente.");
        }
        catch (Exception ex)
        {
            AddUnexpectedModelError("Erro ao cadastrar paciente", ex);
        }

        return View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var paciente = await _service.ObterPorIdAsync(id);

            if (paciente == null)
                return RedirectToIndexWithError("Paciente não encontrado.");

            return View(paciente);
        }
        catch (Exception ex)
        {
            return RedirectToIndexWithError($"Erro ao carregar paciente: {ex.Message}");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PacienteViewModel model)
    {
        if (id != model.Id)
            return RedirectToIndexWithError("ID inválido.");

        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var sucesso = await _service.AtualizarAsync(id, model);

            if (sucesso)
            {
                TempData["Sucesso"] = "Paciente atualizado com sucesso.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Erro ao atualizar paciente.");
        }
        catch (Exception ex)
        {
            AddUnexpectedModelError("Erro ao atualizar paciente", ex);
        }

        return View(model);
    }

    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var paciente = await _service.ObterPorIdAsync(id);

            if (paciente == null)
                return RedirectToIndexWithError("Paciente não encontrado.");

            return View(paciente);
        }
        catch (Exception ex)
        {
            return RedirectToIndexWithError($"Erro ao carregar paciente: {ex.Message}");
        }
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var sucesso = await _service.ExcluirAsync(id);

            TempData[sucesso ? "Sucesso" : "Erro"] = sucesso
                ? "Paciente excluído com sucesso."
                : "Erro ao excluir paciente.";
        }
        catch (Exception ex)
        {
            TempData["Erro"] = $"Erro ao excluir paciente: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
    }
}
