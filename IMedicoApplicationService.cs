using ClinicaMedica.Web.Core.Entities;
using ClinicaMedica.Web.Core.Exceptions;
using ClinicaMedica.Web.Infrastructure.Repositories;

namespace ClinicaMedica.Web.Application.Services
{
    public interface IMedicoApplicationService
    {
        Task<MedicoDto> GetByIdAsync(int id);
        Task<IEnumerable<MedicoDto>> GetAllAsync();
        Task<int> CreateAsync(CreateMedicoCommand command);
        Task UpdateAsync(int id, UpdateMedicoCommand command);
        Task DeleteAsync(int id);
    }

    public class MedicoApplicationService : IMedicoApplicationService
    {
        private readonly IRepository<Medico> _medicoRepository;
        private readonly ILogger<MedicoApplicationService> _logger;

        public MedicoApplicationService(IRepository<Medico> medicoRepository, ILogger<MedicoApplicationService> logger)
        {
            _medicoRepository = medicoRepository;
            _logger = logger;
        }

        public async Task<MedicoDto> GetByIdAsync(int id)
        {
            var medico = await _medicoRepository.GetByIdAsync(id) 
                ?? throw new DomainException($"Médico com ID {id} não encontrado.");

            return MedicoDto.FromEntity(medico);
        }

        public async Task<IEnumerable<MedicoDto>> GetAllAsync()
        {
            var medicos = await _medicoRepository.GetAllAsync();
            return medicos.Select(MedicoDto.FromEntity);
        }

        public async Task<int> CreateAsync(CreateMedicoCommand command)
        {
            try
            {
                var novoMedico = new Medico(
                    command.Nome,
                    command.Crm,
                    command.Especialidade,
                    command.Telefone,
                    command.Email,
                    command.Ativo
                );

                var sucesso = await _medicoRepository.AddAsync(novoMedico);
                
                if (!sucesso)
                    throw new DomainException("Erro ao adicionar médico no banco de dados.");

                _logger.LogInformation($"Médico criado com sucesso: CRM {novoMedico.Crm}");
                return novoMedico.Id;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao criar médico: {ex.Message}");
                throw new DomainException("Erro ao criar médico.", ex);
            }
        }

        public async Task UpdateAsync(int id, UpdateMedicoCommand command)
        {
            try
            {
                var medico = await _medicoRepository.GetByIdAsync(id)
                    ?? throw new DomainException($"Médico com ID {id} não encontrado.");

                medico.Atualizar(
                    command.Nome,
                    command.Crm,
                    command.Especialidade,
                    command.Telefone,
                    command.Email,
                    command.Ativo
                );

                var sucesso = await _medicoRepository.UpdateAsync(medico);
                
                if (!sucesso)
                    throw new DomainException("Erro ao atualizar médico no banco de dados.");

                _logger.LogInformation($"Médico atualizado com sucesso: ID {id}");
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao atualizar médico ID {id}: {ex.Message}");
                throw new DomainException("Erro ao atualizar médico.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var medico = await _medicoRepository.GetByIdAsync(id)
                    ?? throw new DomainException($"Médico com ID {id} não encontrado.");

                var sucesso = await _medicoRepository.DeleteAsync(id);
                
                if (!sucesso)
                    throw new DomainException("Erro ao deletar médico do banco de dados.");

                _logger.LogInformation($"Médico deletado com sucesso: ID {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao deletar médico ID {id}: {ex.Message}");
                throw new DomainException("Erro ao deletar médico.", ex);
            }
        }
    }

    public class MedicoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Crm { get; set; } = string.Empty;
        public string Especialidade { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }

        public static MedicoDto FromEntity(Medico medico) => new()
        {
            Id = medico.Id,
            Nome = medico.Nome,
            Crm = medico.Crm,
            Especialidade = medico.Especialidade,
            Telefone = medico.Telefone,
            Email = medico.Email,
            Ativo = medico.Ativo,
            DataCadastro = medico.DataCadastro
        };
    }

    public class CreateMedicoCommand
    {
        public string Nome { get; set; } = string.Empty;
        public string Crm { get; set; } = string.Empty;
        public string Especialidade { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
    }

    public class UpdateMedicoCommand
    {
        public string Nome { get; set; } = string.Empty;
        public string Crm { get; set; } = string.Empty;
        public string Especialidade { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Ativo { get; set; }
    }
}