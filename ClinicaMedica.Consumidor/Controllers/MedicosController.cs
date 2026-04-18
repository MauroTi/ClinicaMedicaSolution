using ClinicaMedica.Consumidor.Helpers;
using ClinicaMedica.Consumidor.Services.Interfaces;
using ClinicaMedica.Consumidor.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedica.Consumidor.Controllers;

public class MedicosController : ConsumerControllerBase
{
    private const int PageSize = 10;
    private readonly IMedicoService _service;

    public MedicosController(IMedicoService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        try
        {
            var medicos = await _service.ObterTodosAsync();
            var pagination = CreatePagination(page, medicos.Count, PageSize);

            var model = new MedicoIndexViewModel
            {
                Medicos = PaginationHelper.Slice(medicos, pagination.CurrentPage, PageSize),
                Pagination = pagination
            };

            return View(model);
        }
        catch (Exception ex)
        {
            ViewData["ErroApi"] = $"Erro ao carregar médicos: {ex.Message}";
            return View(new MedicoIndexViewModel
            {
                Pagination = CreatePagination(1, 0, PageSize)
            });
        }
    }

    public IActionResult Create()
    {
        return View(new MedicoViewModel
        {
            Ativo = true,
            DataCadastro = DateTime.Now
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MedicoViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            model.DataCadastro = DateTime.Now;

            var sucesso = await _service.CriarAsync(model);

            if (sucesso)
            {
                TempData["Sucesso"] = "Médico cadastrado com sucesso.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Não foi possível cadastrar o médico.");
        }
        catch (Exception ex)
        {
            AddUnexpectedModelError("Erro ao cadastrar médico", ex);
        }

        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var medico = await _service.ObterPorIdAsync(id);

            if (medico == null)
                return RedirectToIndexWithError("Médico não encontrado.");

            return View(medico);
        }
        catch (Exception ex)
        {
            return RedirectToIndexWithError($"Erro ao carregar médico: {ex.Message}");
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var medico = await _service.ObterPorIdAsync(id);

            if (medico == null)
                return RedirectToIndexWithError("Médico não encontrado.");

            return View(medico);
        }
        catch (Exception ex)
        {
            return RedirectToIndexWithError($"Erro ao carregar médico para edição: {ex.Message}");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MedicoViewModel model)
    {
        if (id != model.Id)
            return RedirectToIndexWithError("ID inválido.");

        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var existente = await _service.ObterPorIdAsync(id);

            if (existente == null)
                return RedirectToIndexWithError("Médico não encontrado.");

            model.DataCadastro = existente.DataCadastro;

            var sucesso = await _service.AtualizarAsync(id, model);

            if (sucesso)
            {
                TempData["Sucesso"] = "Médico atualizado com sucesso.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Não foi possível atualizar o médico.");
        }
        catch (Exception ex)
        {
            AddUnexpectedModelError("Erro ao atualizar médico", ex);
        }

        return View(model);
    }

    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var medico = await _service.ObterPorIdAsync(id);

            if (medico == null)
                return RedirectToIndexWithError("Médico não encontrado.");

            return View(medico);
        }
        catch (Exception ex)
        {
            return RedirectToIndexWithError($"Erro ao carregar médico para exclusão: {ex.Message}");
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
                ? "Médico excluído com sucesso."
                : "Não foi possível excluir o médico.";
        }
        catch (Exception ex)
        {
            TempData["Erro"] = $"Erro ao excluir médico: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
    }
}
