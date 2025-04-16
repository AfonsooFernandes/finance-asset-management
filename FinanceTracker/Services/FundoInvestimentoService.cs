using FinanceTracker.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FinanceTracker.Services
{
    public class FundoInvestimentoService
    {
        private readonly HttpClient _httpClient;

        public FundoInvestimentoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<FundoInvestimentoDto>> GetAllFundos()
        {
            var response = await _httpClient.GetAsync("api/fundos");
            
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<IEnumerable<FundoInvestimentoDto>>();

            return new List<FundoInvestimentoDto>();
        }

        public async Task<FundoInvestimentoDto> GetFundoById(int id)
        {
            var response = await _httpClient.GetAsync($"api/fundos/{id}");
            
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<FundoInvestimentoDto>();

            return null;
        }

        public async Task<string> CreateFundo(FundoInvestimentoDto fundo)
        {
            var response = await _httpClient.PostAsJsonAsync("api/fundos", fundo);
            
            if (response.IsSuccessStatusCode)
                return "Fundo de investimento criado com sucesso.";
            
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateFundo(int id, FundoInvestimentoDto fundo)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/fundos/{id}", fundo);
            
            if (response.IsSuccessStatusCode)
                return "Fundo de investimento atualizado com sucesso.";
            
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteFundo(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/fundos/{id}");
            
            if (response.IsSuccessStatusCode)
                return "Fundo de investimento removido com sucesso.";
            
            return await response.Content.ReadAsStringAsync();
        }
    }
}