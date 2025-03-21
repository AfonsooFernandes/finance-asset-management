using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Data;
using FinanceTracker.Models;

namespace FinanceTracker.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly FinanceTrackerContext _context;

        public AuthController(FinanceTrackerContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _context.Utilizadores.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
                return Conflict("O email já está em uso.");

            var newUser = new Utilizador
            {
                Nome = request.Nome,
                Email = request.Email,
                SenhaHash = HashPassword(request.Senha),
                TipoUtilizador = "USER"
            };

            _context.Utilizadores.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Utilizador registado com sucesso." });
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
        {
            var user = await _context.Utilizadores
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || user.SenhaHash != HashPassword(loginDto.Senha))
            {
                return BadRequest("Email ou senha inválidos.");
            }

            return RedirectToPage("/SuccessLogin");
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}