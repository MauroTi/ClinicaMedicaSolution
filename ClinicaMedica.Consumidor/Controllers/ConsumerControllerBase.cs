using ClinicaMedica.Consumidor.Helpers;
using ClinicaMedica.Consumidor.Infrastructure.Database;
using ClinicaMedica.Consumidor.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedica.Consumidor.Controllers;

public abstract class ConsumerControllerBase : Controller
{
    protected PaginationViewModel CreatePagination(int page, int totalItems, int pageSize)
    {
        var database = HttpContext.Session.GetString(DatabaseSession.SessionKey);
        return PaginationHelper.Create(page, totalItems, pageSize, database);
    }

    protected RedirectToActionResult RedirectToIndexWithError(string message)
    {
        TempData["Erro"] = message;
        return RedirectToAction("Index")!;
    }

    protected void AddUnexpectedModelError(string operation, Exception exception)
    {
        ModelState.AddModelError(string.Empty, $"{operation}: {exception.Message}");
    }
}
