using Microsoft.AspNetCore.Mvc;
using ClinicaMedica.Web.Infrastructure.Database.Providers;
using ClinicaMedica.Web.Data;

namespace ClinicaMedica.Web.Controllers
{
    [Route("Database")]
    public class DatabaseController : Controller
    {
        private readonly DbConnectionFactory _connectionFactory;

        public DatabaseController(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        [HttpPost("SetDatabaseProvider")]
        public IActionResult SetDatabaseProvider(string provider)
        {
            if (!Enum.TryParse<DatabaseProvider>(provider, true, out var parsed))
                return BadRequest(new { message = "Provider inválido" });

            try
            {
                // salva cookie
                Response.Cookies.Append("SelectedDatabaseProvider", parsed.ToString(), new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(30),
                    HttpOnly = false,
                    IsEssential = true,
                    SameSite = SameSiteMode.Lax
                });

                // testa conexão
                using var conn = _connectionFactory.CreateOpenConnection();

                return Ok();
            }
            catch
            {
                return BadRequest(new { message = "Banco indisponível" });
            }
        }

        [HttpGet("TestConnection")]
        public IActionResult TestConnection()
        {
            try
            {
                using var conn = _connectionFactory.CreateOpenConnection();
                return Ok(new { success = true });
            }
            catch
            {
                return Ok(new { success = false });
            }
        }
    }
}