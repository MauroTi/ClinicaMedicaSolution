using ClinicaMedica.Consumidor.Helpers;
using ClinicaMedica.Consumidor.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedica.Consumidor.Controllers;

public class MedicosController : Controller
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
            var pagination = PaginationHelper.Create(page, medicos.Count, PageSize, HttpContext.Session.GetString("database"));

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
                Pagination = PaginationHelper.Create(1, 0, PageSize, HttpContext.Session.GetString("database"))
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
        {
            return View(model);
        }

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
            ModelState.AddModelError(string.Empty, $"Erro ao cadastrar médico: {ex.Message}");
        }

        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var medico = await _service.ObterPorIdAsync(id);

            if (medico == null)
            {
                TempData["Erro"] = "Médico não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            return View(medico);
        }
        catch (Exception ex)
        {
            TempData["Erro"] = $"Erro ao carregar médico: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var medico = await _service.ObterPorIdAsync(id);

            if (medico == null)
            {
                TempData["Erro"] = "Médico não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            return View(medico);
        }
        catch (Exception ex)
        {
            TempData["Erro"] = $"Erro ao carregar médico para edição: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MedicoViewModel model)
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
            var existente = await _service.ObterPorIdAsync(id);

            if (existente == null)
            {
                TempData["Erro"] = "Médico não encontrado.";
                return RedirectToAction(nameof(Index));
            }

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
            ModelState.AddModelError(string.Empty, $"Erro ao atualizar médico: {ex.Message}");
        }

        return View(model);
    }

    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var medico = await _service.ObterPorIdAsync(id);

            if (medico == null)
            {
                TempData["Erro"] = "Médico não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            return View(medico);
        }
        catch (Exception ex)
        {
            TempData["Erro"] = $"Erro ao carregar médico para exclusão: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var sucesso = await _service.ExcluirAsync(id);

            if (sucesso)
            {
                TempData["Sucesso"] = "Médico excluído com sucesso.";
            }
            else
            {
                TempData["Erro"] = "Não foi possível excluir o médico.";
            }
        }
        catch (Exception ex)
        {
            TempData["Erro"] = $"Erro ao excluir médico: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
    }
}
