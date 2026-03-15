using ClinicaMedica.Web.DTOs;
using ClinicaMedica.Web.Models;
using ClinicaMedica.Web.Models.DTOs;
using ClinicaMedica.Web.ViewModels.Consultas;
using ClinicaMedica.Web.ViewModels.Medicos;
using ClinicaMedica.Web.ViewModels.Pacientes;
namespace ClinicaMedica.Web.Helpers
{
    public static class MappingHelper
    {
        public static MedicoDto ToDto(Medico medico)
        {
            return new MedicoDto
            {
                Id = medico.Id,
                Nome = medico.Nome,
                Especialidade = medico.Especialidade,
                Crm = medico.Crm,
                Telefone = medico.Telefone,
                Email = medico.Email,
                Ativo = medico.Ativo,
                DataCadastro = medico.DataCadastro
            };
        }

        public static PacienteDto ToDto(Paciente paciente)
        {
            return new PacienteDto
            {
                Id = paciente.Id,
                Nome = paciente.Nome,
                Cpf = paciente.Cpf,
                Telefone = paciente.Telefone,
                Email = paciente.Email,
                DataNascimento = paciente.DataNascimento,
                Ativo = paciente.Ativo,
                DataCadastro = paciente.DataCadastro
            };
        }

        public static ConsultaDto ToDto(Consulta consulta, string pacienteNome = "", string medicoNome = "")
        {
            return new ConsultaDto
            {
                Id = consulta.Id,
                MedicoId = consulta.MedicoId,
                MedicoNome = medicoNome,
                PacienteId = consulta.PacienteId,
                PacienteNome = pacienteNome,
                DataHoraConsulta = consulta.DataHoraConsulta,
                Valor = consulta.Valor,
                Status = consulta.Status,
                Observacoes = consulta.Observacoes,
                DataCadastro = consulta.DataCadastro
            };
        }
    }
}