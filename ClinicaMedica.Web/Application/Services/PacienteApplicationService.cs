using ClinicaMedica.Web.Core.Entities;
using ClinicaMedica.Web.Core.Exceptions;
using ClinicaMedica.Web.Infrastructure.Repositories;

namespace ClinicaMedica.Web.Application.Services
{
    public interface IPacienteApplicationService
    {
        Task<PacienteDto> GetByIdAsync(int id);
        Task<IEnumerable<PacienteDto>> GetAllAsync();
        Task<int> CreateAsync(CreatePacienteCommand command);
        Task UpdateAsync(int id, UpdatePacienteCommand command);
        Task DeleteAsync(int id);
    }

    public class PacienteApplicationService : IPacienteApplicationService
    {
        private readonly IRepository<Paciente> _pacienteRepository;
        private readonly ILogger<PacienteApplicationService> _logger;

        public PacienteApplicationService(IRepository<Paciente> pacienteRepository, ILogger<PacienteApplicationService> logger)
        {
            _pacienteRepository = pacienteRepository;
            _logger = logger;
        }

        public async Task<PacienteDto> GetByIdAsync(int id)
        {
            var paciente = await _pacienteRepository.GetByIdAsync(id)
                ?? throw new DomainException($"Paciente com ID {id} não encontrado.");

            return PacienteDto.FromEntity(paciente);
        }

        public async Task<IEnumerable<PacienteDto>> GetAllAsync()
        {
            var pacientes = await _pacienteRepository.GetAllAsync();
            return pacientes.Select(PacienteDto.FromEntity);
        }

        public async Task<int> CreateAsync(CreatePacienteCommand command)
        {
            try
            {
                var novoPaciente = new Paciente(
                    command.Nome,
                    command.Cpf,
                    command.DataNascimento,
                    command.Genero,
                    command.Telefone,
                    command.Email,
                    command.Endereco,
                    command.Cidade,
                    command.Estado,
                    command.Cep,
                    command.Ativo
                );

                var sucesso = await _pacienteRepository.AddAsync(novoPaciente);

                if (!sucesso)
                    throw new DomainException("Erro ao adicionar paciente no banco de dados.");

                _logger.LogInformation($"Paciente criado com sucesso: CPF {novoPaciente.Cpf}");
                return novoPaciente.Id;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao criar paciente: {ex.Message}");
                throw new DomainException("Erro ao criar paciente.", ex);
            }
        }

        public async Task UpdateAsync(int id, UpdatePacienteCommand command)
        {
            try
            {
                var paciente = await _pacienteRepository.GetByIdAsync(id)
                    ?? throw new DomainException($"Paciente com ID {id} não encontrado.");

                paciente.Atualizar(
                    command.Nome,
                    command.Cpf,
                    command.DataNascimento,
                    command.Genero,
                    command.Telefone,
                    command.Email,
                    command.Endereco,
                    command.Cidade,
                    command.Estado,
                    command.Cep,
                    command.Ativo
                );

                var sucesso = await _pacienteRepository.UpdateAsync(paciente);

                if (!sucesso)
                    throw new DomainException("Erro ao atualizar paciente no banco de dados.");

                _logger.LogInformation($"Paciente atualizado com sucesso: ID {id}");
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao atualizar paciente ID {id}: {ex.Message}");
                throw new DomainException("Erro ao atualizar paciente.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var paciente = await _pacienteRepository.GetByIdAsync(id)
                    ?? throw new DomainException($"Paciente com ID {id} não encontrado.");

                var sucesso = await _pacienteRepository.DeleteAsync(id);

                if (!sucesso)
                    throw new DomainException("Erro ao deletar paciente do banco de dados.");

                _logger.LogInformation($"Paciente deletado com sucesso: ID {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao deletar paciente ID {id}: {ex.Message}");
                throw new DomainException("Erro ao deletar paciente.", ex);
            }
        }
    }

    public class PacienteDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public int Idade { get; set; }
        public string Genero { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }

        public static PacienteDto FromEntity(Paciente paciente) => new()
        {
            Id = paciente.Id,
            Nome = paciente.Nome,
            Cpf = paciente.Cpf,
            DataNascimento = paciente.DataNascimento,
            Idade = paciente.CalcularIdade(),
            Genero = paciente.Genero,
            Telefone = paciente.Telefone,
            Email = paciente.Email,
            Endereco = paciente.Endereco,
            Cidade = paciente.Cidade,
            Estado = paciente.Estado,
            Cep = paciente.Cep,
            Ativo = paciente.Ativo,
            DataCadastro = paciente.DataCadastro
        };
    }

    public class CreatePacienteCommand
    {
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public string Genero { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
    }

    public class UpdatePacienteCommand
    {
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public string Genero { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public bool Ativo { get; set; }
    }
}