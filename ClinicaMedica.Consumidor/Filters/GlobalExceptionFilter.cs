using ClinicaMedica.Consumidor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Infrastructure;

namespace ClinicaMedica.Consumidor.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (IsApiRequest(context))
        {
            context.Result = new JsonResult(new
            {
                error = "Erro interno no sistema",
                detail = context.Exception.Message
            })
            {
                StatusCode = 500
            };
        }
        else
        {
            var viewData = new ViewDataDictionary(
                new EmptyModelMetadataProvider(),
                context.ModelState)
            {
                Model = new ErrorViewModel
                {
                    RequestId = context.HttpContext.TraceIdentifier
                }
            };

            viewData["ErrorMessage"] = context.Exception.Message;

            context.Result = new ViewResult
            {
                ViewName = "Error",
                ViewData = viewData
            };
        }

        context.ExceptionHandled = true;
    }

    private static bool IsApiRequest(ExceptionContext context)
    {
        var path = context.HttpContext.Request.Path;
        var acceptsJson = context.HttpContext.Request.Headers.Accept
            .Any(value => value?.Contains("application/json", StringComparison.OrdinalIgnoreCase) == true);

        return path.StartsWithSegments("/api") || acceptsJson;
    }
}
