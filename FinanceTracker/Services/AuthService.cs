using FinanceTracker.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FinanceTracker.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> RegisterUser(RegisterUserDto user)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", user);
            
            if (response.IsSuccessStatusCode)
                return "Utilizador registado com sucesso.";
            
            return await response.Content.ReadAsStringAsync();
        }
        
        
    }
}