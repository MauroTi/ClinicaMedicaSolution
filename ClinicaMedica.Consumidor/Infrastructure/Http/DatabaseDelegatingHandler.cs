namespace ClinicaMedica.Consumidor.Infrastructure.Http;

public class DatabaseDelegatingHandler : DelegatingHandler
{
    private readonly IUserContext _userContext;

    public DatabaseDelegatingHandler(IUserContext userContext)
    {
        _userContext = userContext;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.Headers.Remove("X-Database");
        request.Headers.TryAddWithoutValidation("X-Database", _userContext.GetDatabase());

        return base.SendAsync(request, cancellationToken);
    }
}
