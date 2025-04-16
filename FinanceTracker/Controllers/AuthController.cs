using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FinanceTracker.Models;
using FinanceTracker.Services;

namespace FinanceTracker.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthenticationService _authService;

        public AuthController(AuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _authService.EmailExiste(request.Email))
                return Conflict("O email já está em uso.");

            var newUser = new Utilizador
            {
                Nome = request.Nome,
                Email = request.Email,
                SenhaHash = _authService.HashPassword(request.Senha),
                TipoUtilizador = "USER"
            };

            await _authService.CriarUtilizador(newUser);
            return Ok(new { message = "Utilizador registado com sucesso." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
        {
            var valido = await _authService.ValidarCredenciais(loginDto.Email, loginDto.Senha);
            if (!valido)
                return BadRequest("Email ou palavra-passe incorretos.");

            return Ok(new { message = "Login efetuado com sucesso." });
        }
    }
}