using ClinicaMedica.Consumidor.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ClinicaMedica.Consumidor.Filters;

public class DatabaseFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var httpContext = context.HttpContext;
        var databaseFromQuery = httpContext.Request.Query[DatabaseSession.SessionKey].ToString();
        var normalizedDatabase = DatabaseSession.Normalize(databaseFromQuery);

        if (string.IsNullOrWhiteSpace(databaseFromQuery))
        {
            normalizedDatabase = DatabaseSession.Normalize(httpContext.Session.GetString(DatabaseSession.SessionKey));
        }

        httpContext.Session.SetString(DatabaseSession.SessionKey, normalizedDatabase);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

}
