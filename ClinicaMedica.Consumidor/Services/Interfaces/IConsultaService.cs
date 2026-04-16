using ClinicaMedica.Consumidor.ViewModels;

namespace ClinicaMedica.Consumidor.Services.Interfaces;

public interface IConsultaService
{
    Task<List<ConsultaViewModel>> ObterTodosAsync();
}