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

        [BindProperty(SupportsGet = true)]
        public int UserId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? DataInicio { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? DataFim { get; set; }

        public List<RelatorioBancosDto> RelatorioBancos { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var utilizador = await _context.Utilizadores.FindAsync(UserId);

            if (utilizador == null || utilizador.TipoUtilizador != "ADMIN")
                return Unauthorized();

            if (DataInicio == null || DataFim == null)
            {
                RelatorioBancos = new List<RelatorioBancosDto>();
                return Page();
            }

            var dataInicioUtc = DateTime.SpecifyKind(DataInicio.Value, DateTimeKind.Utc);
            var dataFimUtc = DateTime.SpecifyKind(DataFim.Value, DateTimeKind.Utc);

            RelatorioBancos = await _context.DepositosPrazo
                .Include(d => d.AtivoFinanceiro)
                .Where(d =>
                    d.AtivoFinanceiro.DataInicio <= dataFimUtc &&
                    d.AtivoFinanceiro.DataInicio.AddMonths(d.AtivoFinanceiro.Duracao) >= dataInicioUtc
                )
                .GroupBy(d => d.Banco)
                .Select(g => new RelatorioBancosDto
                {
                    Banco = g.Key,
                    TotalDepositado = g.Sum(d => d.Valor),
                    JurosTotaisPagos = g.Sum(d => d.Valor * (d.TaxaJuroAnual / 100.0) * (d.AtivoFinanceiro.Duracao / 12.0))
                })
                .ToListAsync();

            return Page();
        }
    }
}