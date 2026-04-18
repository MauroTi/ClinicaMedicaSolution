namespace ClinicaMedica.Consumidor.Services.Interfaces;

public interface IApiService
{
    Task<T?> GetAsync<T>(string endpoint);
    Task<List<T>> GetAllAsync<T>(string endpoint);
    Task<T?> GetByIdAsync<T>(string endpoint, int id);
    Task<bool> PostAsync<T>(string endpoint, T data);
    Task<bool> PutAsync<T>(string endpoint, int id, T data);
    Task<bool> DeleteAsync(string endpoint, int id);
}
