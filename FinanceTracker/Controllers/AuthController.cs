using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FinanceTracker.Models;
using FinanceTracker.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Collections.Generic;

namespace FinanceTracker.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
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
            var user = await _authService.ValidarCredenciais(loginDto.Email, loginDto.Senha);
            if (user == null)
                return BadRequest("Email ou palavra-passe incorretos.");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("UserId", user.Id.ToString()),
                new Claim("UserType", user.TipoUtilizador)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Ok(new { message = "Login efetuado com sucesso.", userId = user.Id });
        }

        [HttpGet("TipoUtilizador")]
        public async Task<IActionResult> GetTipoUtilizador([FromQuery] int userId)
        {
            var user = await _authService.ObterUtilizadorPorId(userId);
            if (user == null)
                return NotFound();

            return Ok(new { tipoUtilizador = user.TipoUtilizador });
        }
    }
}