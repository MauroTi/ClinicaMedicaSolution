using ClinicaMedica.Consumidor.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ClinicaMedica.Consumidor.Services;
using ClinicaMedica.Consumidor.Services.Implementations;

namespace ClinicaMedica.Consumidor.Controllers
{
    public class ConsultasController : Controller
    {
        private readonly ConsultaService _consultaService;

        public ConsultasController(ConsultaService consultaService)
        {
            _consultaService = consultaService;
        }

        // ================= INDEX =================

        public async Task<IActionResult> Index()
        {
            try
            {
                var consultas = await _consultaService.ObterTodosAsync();
                return View(consultas);
            }
            catch (Exception ex)
            {
                ViewData["ErroApi"] = $"Erro ao carregar consultas: {ex.Message}";
                return View(new List<ConsultaViewModel>());
            }
        }

        // ================= DETAILS =================

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var consulta = await _consultaService.ObterPorIdAsync(id);

                if (consulta == null)
                {
                    TempData["Erro"] = "Consulta não encontrada.";
                    return RedirectToAction(nameof(Index));
                }

                return View(consulta);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao carregar detalhes da consulta: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // ================= CREATE =================

        public async Task<IActionResult> Create()
        {
            var model = new ConsultaViewModel
            {
                DataHoraConsulta = DateTime.Now,
                DataCadastro = DateTime.Now,
                Status = "Agendada"
            };

            try
            {
                await _consultaService.PreencherListasAsync(model);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao carregar dados auxiliares: {ex.Message}";
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ConsultaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await _consultaService.PreencherListasAsync(model);
                return View(model);
            }

            try
            {
                model.DataCadastro = DateTime.Now;

                var sucesso = await _consultaService.CriarAsync(model);

                if (sucesso)
                {
                    TempData["Sucesso"] = "Consulta cadastrada com sucesso!";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, "Não foi possível cadastrar a consulta.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Erro ao cadastrar consulta: {ex.Message}");
            }

            await _consultaService.PreencherListasAsync(model);
            return View(model);
        }

        // ================= EDIT =================

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var consulta = await _consultaService.ObterPorIdAsync(id);

                if (consulta == null)
                {
                    TempData["Erro"] = "Consulta não encontrada.";
                    return RedirectToAction(nameof(Index));
                }

                await _consultaService.PreencherListasAsync(consulta);
                return View(consulta);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao carregar consulta para edição: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ConsultaViewModel model)
        {
            if (id != model.Id)
            {
                TempData["Erro"] = "ID inválido.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                await _consultaService.PreencherListasAsync(model);
                return View(model);
            }

            try
            {
                var existente = await _consultaService.ObterPorIdAsync(id);

                if (existente == null)
                {
                    TempData["Erro"] = "Consulta não encontrada.";
                    return RedirectToAction(nameof(Index));
                }

                model.DataCadastro = existente.DataCadastro;

                var sucesso = await _consultaService.AtualizarAsync(id, model);

                if (sucesso)
                {
                    TempData["Sucesso"] = "Consulta atualizada com sucesso!";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, "Não foi possível atualizar a consulta.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Erro ao atualizar consulta: {ex.Message}");
            }

            await _consultaService.PreencherListasAsync(model);
            return View(model);
        }

        // ================= DELETE =================

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var consulta = await _consultaService.ObterPorIdAsync(id);

                if (consulta == null)
                {
                    TempData["Erro"] = "Consulta não encontrada.";
                    return RedirectToAction(nameof(Index));
                }

                return View(consulta);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao carregar consulta para exclusão: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var sucesso = await _consultaService.ExcluirAsync(id);

                if (sucesso)
                {
                    TempData["Sucesso"] = "Consulta excluída com sucesso!";
                }
                else
                {
                    TempData["Erro"] = "Não foi possível excluir a consulta.";
                }
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao excluir consulta: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // ================= GRÁFICO =================

        public async Task<IActionResult> GraficoStatus()
        {
            try
            {
                var dados = await _consultaService.ObterGraficoStatusAsync();
                return View(dados);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao carregar gráfico: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}