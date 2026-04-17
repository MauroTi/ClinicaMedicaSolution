using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;

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

            var query = ctx.Request.Query["database"].ToString();

            if (!string.IsNullOrWhiteSpace(query) &&
                Enum.TryParse(query, true, out DatabaseProvider qp))
            {
                return qp;
            }

            // 🔥 1. HEADER (PRIORIDADE REAL DO CONSUMER)
            if (ctx.Request.Headers.TryGetValue("X-Database", out var headerValue))
            {
                var header = headerValue.ToString();

                if (!string.IsNullOrWhiteSpace(header) &&
                    Enum.TryParse(header, true, out DatabaseProvider headerProvider))
                {
                    Console.WriteLine($"🔥 DB via HEADER: {headerProvider}");
                    return headerProvider;
                }
            }

            // 🔥 2. COOKIE (fallback UI)
            if (ctx.Request.Cookies.TryGetValue(CookieName, out var cookieValue))
            {
                if (!string.IsNullOrWhiteSpace(cookieValue) &&
                    Enum.TryParse(cookieValue, true, out DatabaseProvider cookieProvider))
                {
                    Console.WriteLine($"🍪 DB via COOKIE: {cookieProvider}");
                    return cookieProvider;
                }
            }

            // 🔥 3. CONFIG (fallback final)
            var configValue = _configuration["DatabaseSettings:DefaultProvider"];

            if (!string.IsNullOrWhiteSpace(configValue) &&
                Enum.TryParse(configValue, true, out DatabaseProvider configProvider))
            {
                Console.WriteLine($"⚙️ DB via CONFIG: {configProvider}");
                return configProvider;
            }

            Console.WriteLine("⚠️ DB fallback: MySql");
            return DatabaseProvider.MySql;
        }
    }
}