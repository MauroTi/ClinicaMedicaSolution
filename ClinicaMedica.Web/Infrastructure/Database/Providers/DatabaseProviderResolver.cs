using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ClinicaMedica.Web.Infrastructure.Database.Providers
{
    public class DatabaseProviderResolver : IDatabaseProviderResolver
    {
        private const string CookieName = "SelectedDatabaseProvider";

        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<DatabaseProviderResolver> _logger;

        public DatabaseProviderResolver(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ILogger<DatabaseProviderResolver> logger)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public DatabaseProvider GetProvider()
        {
            var ctx = _httpContextAccessor.HttpContext;

            if (ctx is not null &&
                TryParseProvider(ctx.Request.Query["database"].ToString(), out var queryProvider))
            {
                _logger.LogInformation("Provider resolvido via query string: {Provider}", queryProvider);
                return queryProvider;
            }

            if (ctx?.Request.Headers.TryGetValue("X-Database", out var headerValue) == true &&
                TryParseProvider(headerValue.ToString(), out var headerProvider))
            {
                _logger.LogInformation("Provider resolvido via header: {Provider}", headerProvider);
                return headerProvider;
            }

            if (ctx?.Request.Cookies.TryGetValue(CookieName, out var cookieValue) == true &&
                TryParseProvider(cookieValue, out var cookieProvider))
            {
                _logger.LogInformation("Provider resolvido via cookie: {Provider}", cookieProvider);
                return cookieProvider;
            }

            var configValue = _configuration["DatabaseSettings:Provider"]
                ?? _configuration["DatabaseSettings:DefaultProvider"];

            if (TryParseProvider(configValue, out var configProvider))
            {
                _logger.LogInformation("Provider resolvido via configuração: {Provider}", configProvider);
                return configProvider;
            }

            _logger.LogWarning("Provider não informado. Aplicando fallback para MySql.");
            return DatabaseProvider.MySql;
        }

        private static bool TryParseProvider(string? value, out DatabaseProvider provider)
        {
            provider = default;
            return !string.IsNullOrWhiteSpace(value)
                && Enum.TryParse(value, true, out provider);
        }
    }
}
