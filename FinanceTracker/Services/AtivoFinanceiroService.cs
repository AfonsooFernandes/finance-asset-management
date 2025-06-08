using FinanceTracker.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FinanceTracker.Services
{
    public class AtivoFinanceiroService
    {
        private readonly HttpClient _httpClient;

        public AtivoFinanceiroService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<AtivoFinanceiroDto>> GetAllAtivos()
        {
            var response = await _httpClient.GetAsync("api/ativos");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<AtivoFinanceiroDto>>();
        }

        public async Task<AtivoFinanceiroDto> GetAtivoById(int id)
        {
            var response = await _httpClient.GetAsync($"api/ativos/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AtivoFinanceiroDto>();
        }

        public async Task<AtivoFinanceiroDto> CreateAtivo(AtivoFinanceiroDto ativo)
        {
            var response = await _httpClient.PostAsJsonAsync("api/ativos", ativo);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AtivoFinanceiroDto>();
        }

        public async Task<string> UpdateAtivo(int id, AtivoFinanceiroDto ativo)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/ativos/{id}", ativo);
            response.EnsureSuccessStatusCode();
            return "Ativo financeiro atualizado com sucesso.";
        }

        public async Task<string> DeleteAtivo(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/ativos/{id}");
            response.EnsureSuccessStatusCode();
            return "Ativo financeiro removido com sucesso.";
        }
    }
}