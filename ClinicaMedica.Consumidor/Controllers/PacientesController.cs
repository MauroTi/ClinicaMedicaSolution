using ClinicaMedica.Consumidor.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ClinicaMedica.Consumidor.Services.Interfaces;

namespace ClinicaMedica.Consumidor.Controllers
{
    public class PacientesController : Controller
    {
        private readonly IApiService _api;
        private readonly string _endpoint = "pacientes";

        public PacientesController(IApiService api)
        {
            _api = api;
        }

        // ================= INDEX =================

        public async Task<IActionResult> Index()
        {
            try
            {
                var pacientes = await _api.GetAllAsync<PacienteViewModel>(_endpoint);
                return View(pacientes);
            }
            catch (Exception ex)
            {
                ViewData["ErroApi"] = $"Erro ao carregar pacientes: {ex.Message}";
                return View(new List<PacienteViewModel>());
            }
        }

        // ================= DETAILS =================

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var paciente = await _api.GetByIdAsync<PacienteViewModel>(_endpoint, id);

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

        // ================= CREATE =================

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
                var sucesso = await _api.PostAsync(_endpoint, model);

                if (sucesso)
                {
                    TempData["Sucesso"] = "Paciente cadastrado com sucesso.";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Erro ao cadastrar paciente.");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro inesperado: {ex.Message}");
                return View(model);
            }
        }

        // ================= EDIT =================

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var paciente = await _api.GetByIdAsync<PacienteViewModel>(_endpoint, id);

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
                return View(model);

            try
            {
                var sucesso = await _api.PutAsync<PacienteViewModel>(_endpoint, id, model);

                if (sucesso)
                {
                    TempData["Sucesso"] = "Paciente atualizado com sucesso.";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Erro ao atualizar paciente.");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Erro inesperado: {ex.Message}");
                return View(model);
            }
        }

        // ================= DELETE =================

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var paciente = await _api.GetByIdAsync<PacienteViewModel>(_endpoint, id);

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
                var sucesso = await _api.DeleteAsync(_endpoint, id);

                if (sucesso)
                {
                    TempData["Sucesso"] = "Paciente excluído com sucesso.";
                }
                else
                {
                    TempData["Erro"] = "Erro ao excluir paciente.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro inesperado: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}