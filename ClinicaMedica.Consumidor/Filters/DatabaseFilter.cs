using Microsoft.AspNetCore.Mvc.Filters;

public class DatabaseFilter : IActionFilter
{
    private readonly IApiService _api;

    public DatabaseFilter(IApiService api)
    {
        _api = api;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var database = context.HttpContext.Request.Query["database"].ToString();
        _api.SetDatabase(database);
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}