using ClinicaMedica.Consumidor.Services;
using ClinicaMedica.Consumidor.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedica.Consumidor.Controllers
{
    public class PacientesController : Controller
    {
        private readonly PacienteService _pacienteService;

        public PacientesController(PacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        // GET: Pacientes
        public async Task<IActionResult> Index()
        {
            try
            {
                var pacientes = await _pacienteService.ObterTodosAsync();
                return View(pacientes);
            }
            catch (Exception ex)
            {
                ViewData["ErroApi"] = $"Erro ao carregar pacientes: {ex.Message}";
                return View(new List<PacienteViewModel>());
            }
        }

        // GET: Pacientes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var paciente = await _pacienteService.ObterPorIdAsync(id);

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

        // GET: Pacientes/Create
        public IActionResult Create()
        {
            var model = new PacienteViewModel
            {
                Ativo = true
            };

            return View(model);
        }

        // POST: Pacientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PacienteViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                if (model.DataCadastro == default)
                    model.DataCadastro = DateTime.Now;

                var response = await _pacienteService.AdicionarAsync(model);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Sucesso"] = "Paciente cadastrado com sucesso.";
                    return RedirectToAction(nameof(Index));
                }

                var erroApi = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Erro ao cadastrar paciente. {erroApi}");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Erro inesperado ao cadastrar paciente: {ex.Message}");
                return View(model);
            }
        }

        // GET: Pacientes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var paciente = await _pacienteService.ObterPorIdAsync(id);

                if (paciente == null)
                {
                    TempData["Erro"] = "Paciente não encontrado.";
                    return RedirectToAction(nameof(Index));
                }

                return View(paciente);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao carregar paciente para edição: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Pacientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PacienteViewModel model)
        {
            if (id != model.Id)
            {
                TempData["Erro"] = "ID inválido para edição.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var pacienteOriginal = await _pacienteService.ObterPorIdAsync(id);
                if (pacienteOriginal != null && model.DataCadastro == default)
                {
                    model.DataCadastro = pacienteOriginal.DataCadastro;
                }

                var response = await _pacienteService.AtualizarAsync(id, model);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Sucesso"] = "Paciente atualizado com sucesso.";
                    return RedirectToAction(nameof(Index));
                }

                var erroApi = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Erro ao atualizar paciente. {erroApi}");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Erro inesperado ao atualizar paciente: {ex.Message}");
                return View(model);
            }
        }

        // GET: Pacientes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var paciente = await _pacienteService.ObterPorIdAsync(id);

                if (paciente == null)
                {
                    TempData["Erro"] = "Paciente não encontrado.";
                    return RedirectToAction(nameof(Index));
                }

                return View(paciente);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao carregar paciente para exclusão: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Pacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var response = await _pacienteService.ExcluirAsync(id);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Sucesso"] = "Paciente excluído com sucesso.";
                    return RedirectToAction(nameof(Index));
                }

                var erroApi = await response.Content.ReadAsStringAsync();
                TempData["Erro"] = $"Erro ao excluir paciente. {erroApi}";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro inesperado ao excluir paciente: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}