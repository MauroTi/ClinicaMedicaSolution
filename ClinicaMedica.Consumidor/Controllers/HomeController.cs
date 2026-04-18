using ClinicaMedica.Consumidor.Services;
using ClinicaMedica.Consumidor.Services.Interfaces;
using ClinicaMedica.Consumidor.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedica.Consumidor.Controllers;

public class HomeController : Controller
{
    private readonly IApiService _apiService;

    public HomeController(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var model = await _apiService.GetAsync<HomeViewModel>(ApiEndpoints.Dashboard);
            return View(model ?? new HomeViewModel());
        }
        catch (Exception ex)
        {
            ViewData["ErroApi"] = ex.Message;
            return View(new HomeViewModel());
        }
    }
}
