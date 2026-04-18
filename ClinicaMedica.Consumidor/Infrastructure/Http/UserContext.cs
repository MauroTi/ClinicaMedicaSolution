using ClinicaMedica.Consumidor.Infrastructure.Database;
using Microsoft.AspNetCore.Http;

namespace ClinicaMedica.Consumidor.Infrastructure.Http;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _http;

    public UserContext(IHttpContextAccessor http)
    {
        _http = http;
    }

    public string GetDatabase()
    {
        var database = _http.HttpContext?.Session?.GetString(DatabaseSession.SessionKey);
        return DatabaseSession.Normalize(database);
    }
}
