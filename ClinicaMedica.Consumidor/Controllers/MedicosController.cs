using ClinicaMedica.Consumidor.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ClinicaMedica.Consumidor.Services.Interfaces;


public class MedicoController : Controller
{
    private readonly IMedicoService _service;
    private readonly IApiService _api;

    public MedicoController(IMedicoService service, IApiService api)
    {
        _service = service;
        _api = api;
    }

    public async Task<IActionResult> Index(string database)
    {
        _api.SetDatabase(database);

        var dados = await _service.ObterTodosAsync();
        return View(dados);
    }
}