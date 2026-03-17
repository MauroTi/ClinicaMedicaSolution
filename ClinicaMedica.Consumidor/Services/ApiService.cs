// ========================= ApiService.cs =========================
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ClinicaMedica.Consumidor.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;

        public ApiService(HttpClient http)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
        }

        // ========================= GENÉRICO =========================

        public async Task<List<T>> GetAllAsync<T>(string endpoint)
        {
            try
            {
                return await _http.GetFromJsonAsync<List<T>>(endpoint) ?? new List<T>();
            }
            catch
            {
                return new List<T>();
            }
        }

        public async Task<T?> GetByIdAsync<T>(string endpoint, int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<T>($"{endpoint}/{id}");
            }
            catch
            {
                return default;
            }
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T obj)
        {
            try
            {
                return await _http.PostAsJsonAsync(endpoint, obj);
            }
            catch
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string endpoint, int id, T obj)
        {
            try
            {
                return await _http.PutAsJsonAsync($"{endpoint}/{id}", obj);
            }
            catch
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }
        }

        public async Task<HttpResponseMessage> DeleteAsync(string endpoint, int id)
        {
            try
            {
                return await _http.DeleteAsync($"{endpoint}/{id}");
            }
            catch
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}