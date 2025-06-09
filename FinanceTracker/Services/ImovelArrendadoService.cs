using FinanceTracker.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FinanceTracker.Services
{
    public class ImovelArrendadoService
    {
        private readonly HttpClient _httpClient;

        public ImovelArrendadoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ImovelArrendadoDto>> GetAllImoveis()
        {
            var response = await _httpClient.GetAsync("api/imoveis");
            
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<IEnumerable<ImovelArrendadoDto>>();

            return new List<ImovelArrendadoDto>();
        }

        public async Task<ImovelArrendadoDto> GetImovelById(int id)
        {
            var response = await _httpClient.GetAsync($"api/imoveis/{id}");
            
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<ImovelArrendadoDto>();

            return null;
        }

        public async Task<string> CreateImovel(ImovelArrendadoDto imovel)
        {
            var response = await _httpClient.PostAsJsonAsync("api/imoveis", imovel);
            
            if (response.IsSuccessStatusCode)
                return "Imóvel arrendado criado com sucesso.";
            
            return await response.Content.ReadAsStringAsync();
        }
        
        public async Task<ImovelArrendadoDto> GetImovelByAtivoId(int ativoId)
        {
            var response = await _httpClient.GetAsync($"api/imoveis/ativo/{ativoId}");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<ImovelArrendadoDto>();

            return null;
        }


        public async Task<string> UpdateImovel(int id, ImovelArrendadoDto imovel)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/imoveis/{id}", imovel);
            
            if (response.IsSuccessStatusCode)
                return "Imóvel arrendado atualizado com sucesso.";
            
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeleteImovel(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/imoveis/{id}");
            
            if (response.IsSuccessStatusCode)
                return "Imóvel arrendado removido com sucesso.";
            
            return await response.Content.ReadAsStringAsync();
        }
    }
}