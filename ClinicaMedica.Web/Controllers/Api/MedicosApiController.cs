using ClinicaMedica.Web.Models;
using ClinicaMedica.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedica.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicosApiController : ControllerBase
    {
        private readonly IMedicoService _medicoService;

        public MedicosApiController(IMedicoService medicoService)
        {
            _medicoService = medicoService;
        }

        // GET: api/Medicos
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var medicos = await _medicoService.ObterTodosAsync();
            return Ok(medicos);
        }

        // GET: api/Medicos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var medico = await _medicoService.ObterPorIdAsync(id);
            if (medico == null)
                return NotFound(new { Mensagem = "Médico não encontrado." });

            return Ok(medico);
        }

        // POST: api/Medicos
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Medico model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _medicoService.AdicionarAsync(model);

            // Retorna CreatedAtAction com o ID do médico criado
            return CreatedAtAction(nameof(Get), new { id }, new { Mensagem = "Médico cadastrado com sucesso." });
        }

        // PUT: api/Medicos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Medico model)
        {
            if (id != model.Id)
                return BadRequest(new { Mensagem = "ID da rota difere do ID do corpo." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verifica se o médico existe antes de atualizar
            var existente = await _medicoService.ObterPorIdAsync(id);
            if (existente == null)
                return NotFound(new { Mensagem = "Médico não encontrado para atualização." });

            var atualizado = await _medicoService.AtualizarAsync(model);
            if (!atualizado)
                return BadRequest(new { Mensagem = "Não foi possível atualizar o médico." });

            return Ok(new { Mensagem = "Médico atualizado com sucesso." });
        }

        // DELETE: api/Medicos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var excluido = await _medicoService.ExcluirAsync(id);

            if (!excluido)
                return BadRequest(new { Mensagem = "Não foi possível excluir o médico." });

            return Ok(new { Mensagem = "Médico excluído com sucesso." });
        }
    }
}