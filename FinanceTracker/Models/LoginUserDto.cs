using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Models
{
    public class LoginUserDto
    {
        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A palavra-passe é obrigatória.")]
        public string Senha { get; set; }
    }
}