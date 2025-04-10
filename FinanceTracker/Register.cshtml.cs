using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FinanceTracker.Models;
using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public RegisterModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [BindProperty]
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O formato do email é inválido.")]
        public string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "A palavra-passe é obrigatória.")]
        [MinLength(6, ErrorMessage = "A palavra-passe deve ter pelo menos 6 caracteres.")]
        public string Senha { get; set; }

        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userDto = new RegisterUserDto
            {
                Nome = Nome,
                Email = Email,
                Senha = Senha
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("http://localhost:5232/api/auth/register", userDto);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("Register");
                }
                else
                {
                    Message = "Erro ao registar. " + await response.Content.ReadAsStringAsync();
                    return Page();
                }
            }
            catch (HttpRequestException ex)
            {
                Message = "Erro na comunicação com a API: " + ex.Message;
                return Page();
            }
        }
    }
}