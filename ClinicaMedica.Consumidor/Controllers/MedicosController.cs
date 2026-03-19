using ClinicaMedica.Consumidor.Services;
using ClinicaMedica.Consumidor.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedica.Consumidor.Controllers
{
    public class MedicosController : Controller
    {
        private readonly MedicoService _medicoService;

        public MedicosController(MedicoService medicoService)
        {
            _medicoService = medicoService;
        }

        // GET: Medicos
        public async Task<IActionResult> Index()
        {
            try
            {
                var medicos = await _medicoService.ObterTodosAsync();
                return View(medicos);
            }
            catch (Exception ex)
            {
                ViewData["ErroApi"] = $"Erro ao carregar médicos: {ex.Message}";
                return View(new List<MedicoViewModel>());
            }
        }

        // GET: Medicos/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var medico = await _medicoService.ObterPorIdAsync(id);

                if (medico == null)
                {
                    TempData["Erro"] = "Médico não encontrado.";
                    return RedirectToAction(nameof(Index));
                }

                return View(medico);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao buscar médico: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Medicos/Create
        public IActionResult Create()
        {
            var model = new MedicoViewModel
            {
                Ativo = true
            };

            return View(model);
        }

        // POST: Medicos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MedicoViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                // Se a API não preencher automaticamente, enviamos um valor seguro
                if (model.DataCadastro == default)
                    model.DataCadastro = DateTime.Now;

                var response = await _medicoService.AdicionarAsync(model);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Sucesso"] = "Médico cadastrado com sucesso.";
                    return RedirectToAction(nameof(Index));
                }

                var erroApi = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Erro ao cadastrar médico. {erroApi}");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Erro inesperado ao cadastrar médico: {ex.Message}");
                return View(model);
            }
        }

        // GET: Medicos/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var medico = await _medicoService.ObterPorIdAsync(id);

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

        // POST: Medicos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MedicoViewModel model)
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
                // Mantém DataCadastro original caso a API exija esse campo
                var medicoOriginal = await _medicoService.ObterPorIdAsync(id);
                if (medicoOriginal != null && model.DataCadastro == default)
                {
                    model.DataCadastro = medicoOriginal.DataCadastro;
                }

                var response = await _medicoService.AtualizarAsync(id, model);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Sucesso"] = "Médico atualizado com sucesso.";
                    return RedirectToAction(nameof(Index));
                }

                var erroApi = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Erro ao atualizar médico. {erroApi}");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Erro inesperado ao atualizar médico: {ex.Message}");
                return View(model);
            }
        }

        // GET: Medicos/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var medico = await _medicoService.ObterPorIdAsync(id);

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

        // POST: Medicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var response = await _medicoService.ExcluirAsync(id);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Sucesso"] = "Médico excluído com sucesso.";
                    return RedirectToAction(nameof(Index));
                }

                var erroApi = await response.Content.ReadAsStringAsync();
                TempData["Erro"] = $"Erro ao excluir médico. {erroApi}";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro inesperado ao excluir médico: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}