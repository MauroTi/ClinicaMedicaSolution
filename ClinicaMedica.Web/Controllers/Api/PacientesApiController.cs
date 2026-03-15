using ClinicaMedica.Web.Models;
using ClinicaMedica.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaMedica.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacientesApiController : ControllerBase
    {
        private readonly PacienteService _pacienteService;

        public PacientesApiController(PacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        // GET: api/Pacientes
        [HttpGet]
        public IActionResult Get()
        {
            var pacientes = _pacienteService.ObterTodos();
            return Ok(pacientes);
        }

        // GET: api/Pacientes/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var paciente = _pacienteService.ObterPorId(id);

            if (paciente == null)
                return NotFound(new { Mensagem = "Paciente não encontrado." });

            return Ok(paciente);
        }

        // POST: api/Pacientes
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Paciente model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _pacienteService.CriarAsync(model);

            return Ok(new { Mensagem = "Paciente cadastrado com sucesso." });
        }

        // PUT: api/Pacientes/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Paciente model)
        {
            if (id != model.Id)
                return BadRequest(new { Mensagem = "ID da rota difere do ID do corpo." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existente = _pacienteService.ObterPorId(id);
            if (existente == null)
                return NotFound(new { Mensagem = "Paciente não encontrado para atualizar." });

            // Atualização mínima (complementando o que falta)
            existente.Nome = model.Nome;
            existente.Cpf = model.Cpf;
            existente.Telefone = model.Telefone;
            existente.Email = model.Email;
            existente.DataNascimento = model.DataNascimento;
            existente.Ativo = model.Ativo;
            existente.DataCadastro = model.DataCadastro;

            return Ok(new { Mensagem = "Paciente atualizado com sucesso." });
        }

        // DELETE: api/Pacientes/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var excluido = _pacienteService.ExcluirAsync(id);

            if (excluido == null)
                return BadRequest(new { Mensagem = "Não foi possível excluir o paciente." });

            return Ok(new { Mensagem = "Paciente excluído com sucesso." });
        }
    }
}