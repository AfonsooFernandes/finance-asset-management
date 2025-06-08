using FinanceTracker.Data;
using FinanceTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Services
{
    public class AuthService
    {
        private readonly FinanceTrackerContext _context;

        public AuthService(FinanceTrackerContext context)
        {
            _context = context;
        }

        public async Task<bool> EmailExiste(string email)
        {
            return await _context.Utilizadores.AnyAsync(u => u.Email == email);
        }

        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        public async Task CriarUtilizador(Utilizador user)
        {
            _context.Utilizadores.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<Utilizador?> ValidarCredenciais(string email, string senha)
        {
            string senhaHash = HashPassword(senha);
            return await _context.Utilizadores
                .FirstOrDefaultAsync(u => u.Email == email && u.SenhaHash == senhaHash);
        }

        public async Task<Utilizador?> ObterUtilizadorPorId(int userId)
        {
            return await _context.Utilizadores.FindAsync(userId);
        }
    }
}