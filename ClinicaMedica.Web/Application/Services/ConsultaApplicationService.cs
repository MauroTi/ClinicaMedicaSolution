using ClinicaMedica.Web.Core.Entities;
using ClinicaMedica.Web.Core.Exceptions;
using ClinicaMedica.Web.Infrastructure.Repositories;

namespace ClinicaMedica.Web.Application.Services
{
    public interface IConsultaApplicationService
    {
        Task<ConsultaDto> GetByIdAsync(int id);
        Task<IEnumerable<ConsultaDto>> GetAllAsync();
        Task<int> CreateAsync(CreateConsultaCommand command);
        Task UpdateAsync(int id, UpdateConsultaCommand command);
        Task DeleteAsync(int id);
    }

    public class ConsultaApplicationService : IConsultaApplicationService
    {
        private readonly IRepository<Consulta> _consultaRepository;
        private readonly ILogger<ConsultaApplicationService> _logger;

        public ConsultaApplicationService(
            IRepository<Consulta> consultaRepository,
            ILogger<ConsultaApplicationService> logger)
        {
            _consultaRepository = consultaRepository;
            _logger = logger;
        }

        public async Task<ConsultaDto> GetByIdAsync(int id)
        {
            var consulta = await _consultaRepository.GetByIdAsync(id)
                ?? throw new DomainException($"Consulta com ID {id} não encontrada.");

            return ConsultaDto.FromEntity(consulta);
        }

        public async Task<IEnumerable<ConsultaDto>> GetAllAsync()
        {
            var consultas = await _consultaRepository.GetAllAsync();
            return consultas.Select(ConsultaDto.FromEntity);
        }

        public async Task<int> CreateAsync(CreateConsultaCommand command)
        {
            try
            {
                var novaConsulta = new Consulta(
                    command.MedicoId,
                    command.PacienteId,
                    command.DataConsulta,
                    command.Motivo
                );

                var sucesso = await _consultaRepository.AddAsync(novaConsulta);

                if (!sucesso)
                    throw new DomainException("Erro ao adicionar consulta no banco de dados.");

                _logger.LogInformation($"Consulta criada com sucesso: ID {novaConsulta.Id}");
                return novaConsulta.Id;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao criar consulta: {ex.Message}");
                throw new DomainException("Erro ao criar consulta.", ex);
            }
        }

        public async Task UpdateAsync(int id, UpdateConsultaCommand command)
        {
            try
            {
                var consulta = await _consultaRepository.GetByIdAsync(id)
                    ?? throw new DomainException($"Consulta com ID {id} não encontrada.");

                consulta.Atualizar(
                    command.DataConsulta,
                    command.Motivo,
                    command.Diagnostico,
                    command.Prescricao,
                    command.Status
                );

                var sucesso = await _consultaRepository.UpdateAsync(consulta);

                if (!sucesso)
                    throw new DomainException("Erro ao atualizar consulta no banco de dados.");

                _logger.LogInformation($"Consulta atualizada com sucesso: ID {id}");
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao atualizar consulta ID {id}: {ex.Message}");
                throw new DomainException("Erro ao atualizar consulta.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var consulta = await _consultaRepository.GetByIdAsync(id)
                    ?? throw new DomainException($"Consulta com ID {id} não encontrada.");

                var sucesso = await _consultaRepository.DeleteAsync(id);

                if (!sucesso)
                    throw new DomainException("Erro ao deletar consulta do banco de dados.");

                _logger.LogInformation($"Consulta deletada com sucesso: ID {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao deletar consulta ID {id}: {ex.Message}");
                throw new DomainException("Erro ao deletar consulta.", ex);
            }
        }
    }

    public class ConsultaDto
    {
        public int Id { get; set; }
        public int MedicoId { get; set; }
        public int PacienteId { get; set; }
        public DateTime DataConsulta { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public string Diagnostico { get; set; } = string.Empty;
        public string Prescricao { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }

        public static ConsultaDto FromEntity(Consulta consulta) => new()
        {
            Id = consulta.Id,
            MedicoId = consulta.MedicoId,
            PacienteId = consulta.PacienteId,
            DataConsulta = consulta.DataConsulta,
            Motivo = consulta.Motivo,
            Diagnostico = consulta.Diagnostico,
            Prescricao = consulta.Prescricao,
            Status = consulta.Status,
            DataCadastro = consulta.DataCadastro
        };
    }

    public class CreateConsultaCommand
    {
        public int MedicoId { get; set; }
        public int PacienteId { get; set; }
        public DateTime DataConsulta { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }

    public class UpdateConsultaCommand
    {
        public DateTime DataConsulta { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public string Diagnostico { get; set; } = string.Empty;
        public string Prescricao { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}