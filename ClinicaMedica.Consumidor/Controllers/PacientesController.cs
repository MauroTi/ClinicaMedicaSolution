using ClinicaMedica.Consumidor.Helpers;
using ClinicaMedica.Consumidor.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedica.Consumidor.Controllers;

public class PacientesController : Controller
{
    private const int PageSize = 10;
    private const string Endpoint = "pacientes";
    private readonly IApiService _api;

    public PacientesController(IApiService api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        try
        {
            var pacientes = await _api.GetAllAsync<PacienteViewModel>(Endpoint);
            var pagination = PaginationHelper.Create(page, pacientes.Count, PageSize, HttpContext.Session.GetString("database"));

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
                Pagination = PaginationHelper.Create(1, 0, PageSize, HttpContext.Session.GetString("database"))
            });
        }
    }

    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var paciente = await _api.GetByIdAsync<PacienteViewModel>(Endpoint, id);

            if (paciente == null)
            {
                TempData["Erro"] = "Paciente não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            return View(paciente);
        }
        catch (Exception ex)
        {
            TempData["Erro"] = $"Erro ao buscar paciente: {ex.Message}";
            return RedirectToAction(nameof(Index));
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
        {
            return View(model);
        }

        try
        {
            var sucesso = await _api.PostAsync(Endpoint, model);

            if (sucesso)
            {
                TempData["Sucesso"] = "Paciente cadastrado com sucesso.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Erro ao cadastrar paciente.");
            return View(model);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro inesperado: {ex.Message}");
            return View(model);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var paciente = await _api.GetByIdAsync<PacienteViewModel>(Endpoint, id);

            if (paciente == null)
            {
                TempData["Erro"] = "Paciente não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            return View(paciente);
        }
        catch (Exception ex)
        {
            TempData["Erro"] = $"Erro ao carregar paciente: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PacienteViewModel model)
    {
        if (id != model.Id)
        {
            TempData["Erro"] = "ID inválido.";
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var sucesso = await _api.PutAsync(Endpoint, id, model);

            if (sucesso)
            {
                TempData["Sucesso"] = "Paciente atualizado com sucesso.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Erro ao atualizar paciente.");
            return View(model);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Erro inesperado: {ex.Message}");
            return View(model);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var paciente = await _api.GetByIdAsync<PacienteViewModel>(Endpoint, id);

            if (paciente == null)
            {
                TempData["Erro"] = "Paciente não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            return View(paciente);
        }
        catch (Exception ex)
        {
            TempData["Erro"] = $"Erro ao carregar paciente: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var sucesso = await _api.DeleteAsync(Endpoint, id);

            if (sucesso)
            {
                TempData["Sucesso"] = "Paciente excluído com sucesso.";
            }
            else
            {
                TempData["Erro"] = "Erro ao excluir paciente.";
            }
        }
        catch (Exception ex)
        {
            TempData["Erro"] = $"Erro inesperado: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
    }
}
