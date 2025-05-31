using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinanceTracker.Data;
using FinanceTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Pages
{
    public class RelatorioBancosAdminModel : PageModel
    {
        private readonly FinanceTrackerContext _context;

        public RelatorioBancosAdminModel(FinanceTrackerContext context)
        {
            _context = context;
        }

        public List<RelatorioBancoDto> RelatorioBancos { get; set; }

        public async Task<IActionResult> OnGetAsync(int userId)
        {
            var utilizador = await _context.Utilizadores.FindAsync(userId);

            if (utilizador == null || utilizador.TipoUtilizador != "ADMIN")
            {
                return Unauthorized();
            }

            RelatorioBancos = await _context.DepositosPrazo
                .GroupBy(d => d.Banco)
                .Select(g => new RelatorioBancoDto
                {
                    Banco = g.Key,
                    TotalDepositado = g.Sum(d => (decimal)d.Valor),
                    JurosTotaisPagos = g.Sum(d => (decimal)d.Valor * 0.02M)
                })
                .ToListAsync();

            return Page();
        }
    }

    public class RelatorioBancoDto
    {
        public string Banco { get; set; }
        public decimal TotalDepositado { get; set; }
        public decimal JurosTotaisPagos { get; set; }
    }
}