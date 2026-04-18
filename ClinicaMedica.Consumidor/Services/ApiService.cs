using ClinicaMedica.Consumidor.Services.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace ClinicaMedica.Consumidor.Services;

public class ApiService : IApiService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _http;

    public ApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        return await SendAsync<T>(() => _http.GetAsync(BuildCollectionUrl(endpoint)));
    }

    public async Task<List<T>> GetAllAsync<T>(string endpoint)
    {
        return await GetAsync<List<T>>(endpoint) ?? [];
    }

    public async Task<T?> GetByIdAsync<T>(string endpoint, int id)
    {
        return await SendAsync<T>(() => _http.GetAsync(BuildByIdUrl(endpoint, id)));
    }

    public async Task<bool> PostAsync<T>(string endpoint, T data)
    {
        return await SendAsync(() => _http.PostAsJsonAsync(BuildCollectionUrl(endpoint), data, JsonOptions));
    }

    public async Task<bool> PutAsync<T>(string endpoint, int id, T data)
    {
        return await SendAsync(() => _http.PutAsJsonAsync(BuildByIdUrl(endpoint, id), data, JsonOptions));
    }

    public async Task<bool> DeleteAsync(string endpoint, int id)
    {
        return await SendAsync(() => _http.DeleteAsync(BuildByIdUrl(endpoint, id)));
    }

    private static async Task<bool> SendAsync(Func<Task<HttpResponseMessage>> sendRequest)
    {
        using var response = await sendRequest();
        await EnsureSuccessAsync(response);
        return true;
    }

    private static async Task<T?> SendAsync<T>(Func<Task<HttpResponseMessage>> sendRequest)
    {
        using var response = await sendRequest();
        return await ReadResponseAsync<T>(response);
    }

    private static string NormalizeEndpoint(string endpoint)
    {
        var normalized = endpoint.Trim().TrimStart('/');

        return normalized.StartsWith("api/", StringComparison.OrdinalIgnoreCase)
            ? normalized
            : $"api/{normalized}";
    }

    private static string BuildCollectionUrl(string endpoint) => NormalizeEndpoint(endpoint);

    private static string BuildByIdUrl(string endpoint, int id) => $"{NormalizeEndpoint(endpoint)}/{id}";

    private static async Task<T?> ReadResponseAsync<T>(HttpResponseMessage response)
    {
        await EnsureSuccessAsync(response);

        if (response.StatusCode == System.Net.HttpStatusCode.NoContent || response.Content == null)
            return default;

        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content))
            return default;

        return JsonSerializer.Deserialize<T>(content, JsonOptions);
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
            return;

        var payload = await response.Content.ReadAsStringAsync();
        var message = ExtractErrorMessage(payload);

        throw new InvalidOperationException(message ?? $"Falha na requisição HTTP: {(int)response.StatusCode} {response.ReasonPhrase}");
    }

    private static string? ExtractErrorMessage(string? payload)
    {
        if (string.IsNullOrWhiteSpace(payload))
            return null;

        try
        {
            using var document = JsonDocument.Parse(payload);
            var root = document.RootElement;

            foreach (var propertyName in new[] { "message", "Mensagem", "error", "detail" })
            {
                if (root.TryGetProperty(propertyName, out var property) &&
                    property.ValueKind == JsonValueKind.String)
                {
                    return property.GetString();
                }
            }
        }
        catch (JsonException)
        {
            return payload;
        }

        return payload;
    }
}
