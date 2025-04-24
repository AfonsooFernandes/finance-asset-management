using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Data;
using FinanceTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Services
{
    public class AuthenticationService
    {
        private readonly FinanceTrackerContext _context;

        public AuthenticationService(FinanceTrackerContext context)
        {
            _context = context;
        }

        public async Task<bool> EmailExiste(string email)
        {
            return await _context.Utilizadores.AnyAsync(u => u.Email == email);
        }

        public async Task CriarUtilizador(Utilizador user)
        {
            _context.Utilizadores.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<Utilizador> ValidarCredenciais(string email, string senha)
        {
            var user = await _context.Utilizadores.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;

            var senhaHash = HashPassword(senha);
            return user.SenhaHash == senhaHash ? user : null;
        }

        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLowerInvariant();
        }
    }
}