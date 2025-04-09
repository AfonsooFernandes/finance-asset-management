using FinanceTracker.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FinanceTracker.Services
{
    public class PagamentoImpostosService
    {
        private readonly HttpClient _httpClient;

        public PagamentoImpostosService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PagamentoImpostosDto>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<PagamentoImpostosDto>>("api/pagamentos");
        }

        public async Task<PagamentoImpostosDto> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<PagamentoImpostosDto>($"api/pagamentos/{id}");
        }

        public async Task<string> AddAsync(PagamentoImpostosDto pagamento)
        {
            var response = await _httpClient.PostAsJsonAsync("api/pagamentos", pagamento);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateAsync(int id, PagamentoImpostosDto pagamento)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/pagamentos/{id}", pagamento);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/pagamentos/{id}");
            return await response.Content.ReadAsStringAsync();
        }
    }
}