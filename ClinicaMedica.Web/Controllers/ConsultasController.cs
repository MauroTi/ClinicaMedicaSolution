using ClinicaMedica.Web.Helpers;
using ClinicaMedica.Web.Models;
using ClinicaMedica.Web.Services.Interfaces;
using ClinicaMedica.Web.ViewModels.Consultas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClinicaMedica.Web.Controllers;

public class ConsultasController : Controller
{
    private const int PageSize = 10;
    private readonly IConsultaService _consultaService;
    private readonly IPacienteService _pacienteService;
    private readonly IMedicoService _medicoService;

    public ConsultasController(
        IConsultaService consultaService,
        IPacienteService pacienteService,
        IMedicoService medicoService)
    {
        _consultaService = consultaService;
        _pacienteService = pacienteService;
        _medicoService = medicoService;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        var consultas = await _consultaService.ObterTodosDetalhadosAsync();
        var lista = consultas.ToList();
        var pagination = PaginationHelper.Create(page, lista.Count, PageSize);

        var viewModel = new ConsultaIndexViewModel
        {
            Consultas = PaginationHelper.Slice(lista, pagination.CurrentPage, PageSize),
            Pagination = pagination
        };

        return View(viewModel);
    }

    public async Task<IActionResult> Create()
    {
        var vm = new ConsultaFormViewModel
        {
            DataHoraConsulta = DateTime.Now,
            Status = "Agendada",
            Valor = 0
        };

        await CarregarCombosAsync(vm);
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ConsultaFormViewModel vm)
    {
        var valorTexto = Request.Form["Valor"].ToString();

        if (string.IsNullOrWhiteSpace(valorTexto))
        {
            ModelState.AddModelError("Valor", "Informe o valor da consulta.");
        }
        else
        {
            ModelState.Remove(nameof(vm.Valor));

            if (decimal.TryParse(valorTexto, System.Globalization.NumberStyles.Any,
                new System.Globalization.CultureInfo("pt-BR"), out decimal valorConvertido))
            {
                vm.Valor = valorConvertido;
            }
            else if (decimal.TryParse(valorTexto, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out valorConvertido))
            {
                vm.Valor = valorConvertido;
            }
            else
            {
                ModelState.AddModelError("Valor", "Informe um valor válido. Ex.: 1500,00");
            }
        }

        if (!ModelState.IsValid)
        {
            await CarregarCombosAsync(vm);
            return View(vm);
        }

        var consulta = new Consulta
        {
            PacienteId = vm.PacienteId,
            MedicoId = vm.MedicoId,
            DataHoraConsulta = vm.DataHoraConsulta,
            Valor = vm.Valor,
            Observacoes = vm.Observacoes,
            Status = vm.Status,
            DataCadastro = DateTime.Now
        };

        await _consultaService.InserirAsync(consulta);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var consulta = await _consultaService.ObterPorIdAsync(id);
        if (consulta == null)
            return NotFound();

        var vm = new ConsultaFormViewModel
        {
            Id = consulta.Id,
            PacienteId = consulta.PacienteId,
            MedicoId = consulta.MedicoId,
            DataHoraConsulta = consulta.DataHoraConsulta,
            Valor = consulta.Valor,
            Observacoes = consulta.Observacoes,
            Status = consulta.Status,
            DataCadastro = consulta.DataCadastro
        };

        await CarregarCombosAsync(vm);
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ConsultaFormViewModel vm)
    {
        var valorTexto = Request.Form["Valor"].ToString();

        if (string.IsNullOrWhiteSpace(valorTexto))
        {
            ModelState.AddModelError("Valor", "Informe o valor da consulta.");
        }
        else
        {
            ModelState.Remove(nameof(vm.Valor));

            if (decimal.TryParse(valorTexto, System.Globalization.NumberStyles.Any,
                new System.Globalization.CultureInfo("pt-BR"), out decimal valorConvertido))
            {
                vm.Valor = valorConvertido;
            }
            else if (decimal.TryParse(valorTexto, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out valorConvertido))
            {
                vm.Valor = valorConvertido;
            }
            else
            {
                ModelState.AddModelError("Valor", "Informe um valor válido. Ex.: 1500,00");
            }
        }

        if (!ModelState.IsValid)
        {
            await CarregarCombosAsync(vm);
            return View(vm);
        }

        var consulta = new Consulta
        {
            Id = vm.Id,
            PacienteId = vm.PacienteId,
            MedicoId = vm.MedicoId,
            DataHoraConsulta = vm.DataHoraConsulta,
            Valor = vm.Valor,
            Observacoes = vm.Observacoes,
            Status = vm.Status,
            DataCadastro = vm.DataCadastro
        };

        bool sucesso = await _consultaService.AtualizarAsync(consulta);
        if (!sucesso)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var consulta = await _consultaService.ObterPorIdAsync(id);
        if (consulta == null)
            return NotFound();

        var vm = new ConsultaFormViewModel
        {
            Id = consulta.Id,
            PacienteId = consulta.PacienteId,
            MedicoId = consulta.MedicoId,
            DataHoraConsulta = consulta.DataHoraConsulta,
            Valor = consulta.Valor,
            Observacoes = consulta.Observacoes,
            Status = consulta.Status,
            DataCadastro = consulta.DataCadastro,
            ModoSomenteLeitura = true
        };

        await CarregarCombosAsync(vm);
        return View(vm);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        bool sucesso = await _consultaService.ExcluirAsync(id);
        if (!sucesso)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var consulta = await _consultaService.ObterPorIdAsync(id);
        if (consulta == null)
            return NotFound();

        var vm = new ConsultaFormViewModel
        {
            Id = consulta.Id,
            PacienteId = consulta.PacienteId,
            MedicoId = consulta.MedicoId,
            DataHoraConsulta = consulta.DataHoraConsulta,
            Valor = consulta.Valor,
            Observacoes = consulta.Observacoes,
            Status = consulta.Status,
            DataCadastro = consulta.DataCadastro,
            ModoSomenteLeitura = true
        };

        await CarregarCombosAsync(vm);
        return View(vm);
    }

    private async Task CarregarCombosAsync(ConsultaFormViewModel vm)
    {
        var pacientes = await _pacienteService.ObterTodosAsync();
        var medicos = await _medicoService.ObterTodosAsync();

        vm.Pacientes = pacientes.Select(p => new SelectListItem
        {
            Value = p.Id.ToString(),
            Text = p.Nome
        });

        vm.Medicos = medicos.Select(m => new SelectListItem
        {
            Value = m.Id.ToString(),
            Text = m.Nome
        });

        vm.StatusDisponiveis = new List<SelectListItem>
        {
            new() { Value = "Agendada", Text = "Agendada" },
            new() { Value = "Confirmada", Text = "Confirmada" },
            new() { Value = "Realizada", Text = "Realizada" },
            new() { Value = "Cancelada", Text = "Cancelada" }
        };
    }
}
