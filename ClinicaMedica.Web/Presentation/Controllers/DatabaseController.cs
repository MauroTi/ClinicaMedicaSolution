using Microsoft.AspNetCore.Mvc;
using ClinicaMedica.Web.Services;

namespace ClinicaMedica.Web.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseController : ControllerBase
    {
        private readonly IDatabaseConfigurationService _databaseConfigService;

        public DatabaseController(IDatabaseConfigurationService databaseConfigService)
        {
            _databaseConfigService = databaseConfigService;
        }

        /// <summary>
        /// Obtém o provedor de banco de dados atual
        /// </summary>
        [HttpGet("current-provider")]
        public async Task<IActionResult> GetCurrentProvider()
        {
            try
            {
                var provider = await _databaseConfigService.GetCurrentProviderAsync();
                return Ok(new { provider });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao obter provider: {ex.Message}" });
            }
        }

        /// <summary>
        /// Alterna entre provedores de banco de dados (MySQL ou Oracle)
        /// </summary>
        [HttpPost("switch-provider")]
        public async Task<IActionResult> SwitchProvider([FromBody] SwitchProviderRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request?.Provider))
                    return BadRequest(new { message = "Provider é obrigatório" });

                var result = await _databaseConfigService.SwitchProviderAsync(request.Provider);
                
                if (result)
                    return Ok(new { message = $"Provider alterado para {request.Provider}" });
                
                return BadRequest(new { message = "Falha ao alterar provider" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtém informações da conexão com banco de dados
        /// </summary>
        [HttpGet("connection-info")]
        public async Task<IActionResult> GetConnectionInfo()
        {
            try
            {
                var info = await _databaseConfigService.GetConnectionInfoAsync();
                return Ok(info);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro ao obter info de conexão: {ex.Message}" });
            }
        }

        public class SwitchProviderRequest
        {
            public string Provider { get; set; } = string.Empty;
        }
    }
}