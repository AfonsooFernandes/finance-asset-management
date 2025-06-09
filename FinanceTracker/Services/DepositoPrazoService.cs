using FinanceTracker.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FinanceTracker.Services
{
    public class DepositoPrazoService
    {
        private readonly HttpClient _httpClient;

        public DepositoPrazoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<DepositoPrazoDto>> GetAllDepositos()
        {
            var response = await _httpClient.GetAsync("api/depositos");
            
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<IEnumerable<DepositoPrazoDto>>();

            return new List<DepositoPrazoDto>();
        }

        public async Task<DepositoPrazoDto> GetDepositoById(int id)
        {
            var response = await _httpClient.GetAsync($"api/depositos/{id}");
            
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<DepositoPrazoDto>();

            return null;
        }
        
        public async Task<DepositoPrazoDto> GetDepositoByAtivoId(int ativoId)
        {
            // Se sua API tem essa rota
            var response = await _httpClient.GetAsync($"api/depositos/ativo/{ativoId}");
        
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<DepositoPrazoDto>();

            // Caso contrário, busca tudo e filtra localmente
            var depositos = await GetAllDepositos();
            return depositos.FirstOrDefault(d => d.AtivoId == ativoId);
        }


        public async Task<string> CreateDeposito(DepositoPrazoDto deposito)
        {
            var response = await _httpClient.PostAsJsonAsync("api/depositos", deposito);
            
            if (response.IsSuccessStatusCode)
                return "Depósito de prazo criado com sucesso.";
            
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateDeposito(int id, DepositoPrazoDto deposito)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/depositos/{id}", deposito);
            
            if (response.IsSuccessStatusCode)
                return "Depósito de prazo atualizado com sucesso.";
            
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteDeposito(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/depositos/{id}");
            
            if (response.IsSuccessStatusCode)
                return "Depósito de prazo removido com sucesso.";
            
            return await response.Content.ReadAsStringAsync();
        }
    }
}