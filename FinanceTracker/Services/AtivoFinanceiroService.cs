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
            
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<IEnumerable<AtivoFinanceiroDto>>();

            return new List<AtivoFinanceiroDto>();
        }

        public async Task<AtivoFinanceiroDto> GetAtivoById(int id)
        {
            var response = await _httpClient.GetAsync($"api/ativos/{id}");
            
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<AtivoFinanceiroDto>();

            return null;
        }

        public async Task<string> CreateAtivo(AtivoFinanceiroDto ativo)
        {
            var response = await _httpClient.PostAsJsonAsync("api/ativos", ativo);
            
            if (response.IsSuccessStatusCode)
                return "Ativo financeiro criado com sucesso.";
            
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateAtivo(int id, AtivoFinanceiroDto ativo)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/ativos/{id}", ativo);
            
            if (response.IsSuccessStatusCode)
                return "Ativo financeiro atualizado com sucesso.";
            
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteAtivo(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/ativos/{id}");
            
            if (response.IsSuccessStatusCode)
                return "Ativo financeiro removido com sucesso.";
            
            return await response.Content.ReadAsStringAsync();
        }
    }
}
