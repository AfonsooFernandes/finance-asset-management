using FinanceTracker.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FinanceTracker.Services
{
    public class JurosMensaisFundoService
    {
        private readonly HttpClient _httpClient;

        public JurosMensaisFundoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<JurosMensaisFundoDto>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<JurosMensaisFundoDto>>("api/juros");
        }

        public async Task<JurosMensaisFundoDto> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<JurosMensaisFundoDto>($"api/juros/{id}");
        }

        public async Task<string> AddAsync(JurosMensaisFundoDto juros)
        {
            var response = await _httpClient.PostAsJsonAsync("api/juros", juros);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateAsync(int id, JurosMensaisFundoDto juros)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/juros/{id}", juros);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/juros/{id}");
            return await response.Content.ReadAsStringAsync();
        }
    }
}