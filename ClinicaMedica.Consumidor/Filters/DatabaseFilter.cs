using Microsoft.AspNetCore.Mvc.Filters;

public class DatabaseFilter : IActionFilter
{
    private readonly IApiService _api;
    private const string SessionKey = "database";

    public DatabaseFilter(IApiService api)
    {
        _api = api;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var httpContext = context.HttpContext;
        var database = httpContext.Request.Query["database"].ToString();

        if (!string.IsNullOrWhiteSpace(database))
        {
            httpContext.Session.SetString(SessionKey, database);
        }
        else
        {
            database = httpContext.Session.GetString(SessionKey) ?? "mysql";
        }

        _api.SetDatabase(database);
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
