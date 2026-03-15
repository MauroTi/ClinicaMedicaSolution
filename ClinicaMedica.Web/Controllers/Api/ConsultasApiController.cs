using ClinicaMedica.Web.Interfaces;
using ClinicaMedica.Web.Models;
using ClinicaMedica.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedica.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultasController : ControllerBase
    {
        private readonly IConsultaService _consultaService;

        public ConsultasController(IConsultaService consultaService)
        {
            _consultaService = consultaService;
        }

        // GET: api/Consultas
        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var consultas = await _consultaService.ObterTodosDetalhadosAsync();
            return Ok(consultas);
        }

        // GET: api/Consultas/5
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var consulta = await _consultaService.ObterPorIdAsync(id);
            if (consulta == null)
                return NotFound(new { Mensagem = "Consulta não encontrada." });

            return Ok(consulta);
        }

        // POST: api/Consultas
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] Consulta model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _consultaService.InserirAsync(model);
            return CreatedAtAction(nameof(ObterPorId), new { id }, new { Mensagem = "Consulta cadastrada com sucesso." });
        }

        // PUT: api/Consultas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] Consulta model)
        {
            if (id != model.Id)
                return BadRequest(new { Mensagem = "ID da rota difere do ID do corpo." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existente = await _consultaService.ObterPorIdAsync(id);
            if (existente == null)
                return NotFound(new { Mensagem = "Consulta não encontrada para atualização." });

            var atualizado = await _consultaService.AtualizarAsync(model);
            if (!atualizado)
                return BadRequest(new { Mensagem = "Não foi possível atualizar a consulta." });

            return Ok(new { Mensagem = "Consulta atualizada com sucesso." });
        }

        // DELETE: api/Consultas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var excluido = await _consultaService.ExcluirAsync(id);
            if (!excluido)
                return BadRequest(new { Mensagem = "Não foi possível excluir a consulta." });

            return Ok(new { Mensagem = "Consulta excluída com sucesso." });
        }
    }
}