using ClinicaMedica.Web.Models;
using ClinicaMedica.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedica.Web.Controllers.Api
{
    [ApiController]
    [Route("api/pacientes")]
    public class PacientesApiController : ControllerBase
    {
        private readonly IPacienteService _pacienteService;

        public PacientesApiController(IPacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var pacientes = await _pacienteService.ObterTodosAsync();
            return Ok(pacientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var paciente = await _pacienteService.ObterPorIdAsync(id);

            if (paciente == null)
                return NotFound(new { Mensagem = "Paciente não encontrado." });

            return Ok(paciente);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Paciente model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.DataCadastro == default)
                model.DataCadastro = DateTime.Now;

            try
            {
                var id = await _pacienteService.AdicionarAsync(model);

                return CreatedAtAction(nameof(Get), new { id }, new
                {
                    Mensagem = "Paciente cadastrado com sucesso.",
                    Id = id
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Mensagem = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Paciente model)
        {
            if (id != model.Id)
                return BadRequest(new { Mensagem = "ID da rota difere do ID do corpo." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existente = await _pacienteService.ObterPorIdAsync(id);
            if (existente == null)
                return NotFound(new { Mensagem = "Paciente não encontrado para atualizar." });

            if (model.DataCadastro == default)
                model.DataCadastro = existente.DataCadastro;

            try
            {
                var atualizado = await _pacienteService.AtualizarAsync(model);
                if (!atualizado)
                    return BadRequest(new { Mensagem = "Falha ao atualizar o paciente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Mensagem = ex.Message });
            }

            return Ok(new { Mensagem = "Paciente atualizado com sucesso." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var excluido = await _pacienteService.ExcluirAsync(id);

                if (!excluido)
                    return BadRequest(new { Mensagem = "Não foi possível excluir o paciente." });

                return Ok(new { Mensagem = "Paciente excluído com sucesso." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Mensagem = ex.Message });
            }
        }
    }
}
