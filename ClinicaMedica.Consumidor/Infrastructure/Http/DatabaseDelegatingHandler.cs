using System.Net.Http.Headers;

public class DatabaseDelegatingHandler : DelegatingHandler
{
    private readonly IUserContext _userContext;

    public DatabaseDelegatingHandler(IUserContext userContext)
    {
        _userContext = userContext;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var db = _userContext.GetDatabase();

        request.Headers.Remove("X-Database");
        request.Headers.Add("X-Database", db);

        return await base.SendAsync(request, cancellationToken);
    }
}