using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ClinicaMedica.Web.Infrastructure.Database.Providers
{
    public class DatabaseProviderResolver : IDatabaseProviderResolver
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private const string CookieName = "SelectedDatabaseProvider";

        public DatabaseProviderResolver(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public DatabaseProvider GetProvider()
        {
            var ctx = _httpContextAccessor.HttpContext;

            // 🔥 COOKIE PRIORIDADE TOTAL
            var cookie = ctx?.Request?.Cookies[CookieName];

            if (!string.IsNullOrWhiteSpace(cookie) &&
                Enum.TryParse<DatabaseProvider>(cookie, true, out var provider))
            {
                return provider;
            }

            // 🔥 FALLBACK CONFIG
            var configValue = _configuration["DatabaseSettings:DefaultProvider"];

            if (Enum.TryParse<DatabaseProvider>(configValue, true, out var configProvider))
            {
                return configProvider;
            }

            return DatabaseProvider.MySql;
        }
    }
}