// Path: Controllers/Api/MedicosApiController.cs
using ClinicaMedica.Web.Models;
using ClinicaMedica.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ClinicaMedica.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicosApiController : ControllerBase
    {
        private readonly IMedicoService _medicoService;

        public MedicosApiController(IMedicoService medicoService)
        {
            _medicoService = medicoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var medicos = await _medicoService.ObterTodosAsync();
            return Ok(medicos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var medico = await _medicoService.ObterPorIdAsync(id);
            if (medico == null) return NotFound();
            return Ok(medico);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Medico medico)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _medicoService.AdicionarAsync(medico);
                return CreatedAtAction(nameof(GetById), new { id = medico.Id }, medico);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Medico medico)
        {
            if (id != medico.Id) return BadRequest("ID inválido");

            try
            {
                bool sucesso = await _medicoService.AtualizarAsync(medico);
                if (!sucesso) return NotFound();
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool sucesso = await _medicoService.ExcluirAsync(id);
            if (!sucesso) return NotFound();
            return NoContent();
        }
    }
}