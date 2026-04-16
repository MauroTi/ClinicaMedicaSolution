using System.Text;
using System.Text.Json;
using ClinicaMedica.Consumidor.Services.Interfaces;

public class ApiService : IApiService
{
    private readonly HttpClient _http;
    private string _database = "mysql";

    public ApiService(HttpClient http)
    {
        _http = http;
    }

    public void SetDatabase(string database)
    {
        _database = string.IsNullOrWhiteSpace(database)
            ? "mysql"
            : database;
    }

    private string BuildUrl(string endpoint)
    {
        return $"{endpoint}?database={_database}";
    }

    public async Task<List<T>> GetAllAsync<T>(string endpoint)
    {
        var response = await _http.GetAsync(BuildUrl(endpoint));
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new List<T>();
    }

    public async Task<T?> GetByIdAsync<T>(string endpoint, int id)
    {
        var response = await _http.GetAsync($"{BuildUrl(endpoint)}/{id}");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<bool> PostAsync<T>(string endpoint, T data)
    {
        var content = new StringContent(
            JsonSerializer.Serialize(data),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _http.PostAsync(BuildUrl(endpoint), content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> PutAsync<T>(string endpoint, int id, T data)
    {
        var content = new StringContent(
            JsonSerializer.Serialize(data),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _http.PutAsync($"{BuildUrl(endpoint)}/{id}", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(string endpoint, int id)
    {
        var response = await _http.DeleteAsync($"{BuildUrl(endpoint)}/{id}");
        return response.IsSuccessStatusCode;
    }
}