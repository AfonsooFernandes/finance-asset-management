using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Models
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A palavra-passe é obrigatória.")]
        [MinLength(6, ErrorMessage = "A palavra-passe deve ter pelo menos 6 caracteres.")]
        public string Senha { get; set; } = string.Empty;
    }
}