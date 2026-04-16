namespace ClinicaMedica.Consumidor.Infrastructure.Http
{
    using Microsoft.AspNetCore.Http;

    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _http;

        public UserContext(IHttpContextAccessor http)
        {
            _http = http;
        }

        public string GetDatabase()
        {
            var db = _http.HttpContext?.Session?.GetString("database");

            return string.IsNullOrEmpty(db)
                ? "mysql"
                : db;
        }
    }
}