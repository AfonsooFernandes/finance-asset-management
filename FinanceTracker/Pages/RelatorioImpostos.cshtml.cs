using FinanceTracker.Data;
using FinanceTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceTracker.Pages
{
    [Authorize]
    public class RelatorioImpostosModel : PageModel
    {
        private readonly FinanceTrackerContext _context;

        public RelatorioImpostosModel(FinanceTrackerContext context)
        {
            _context = context;
        }

        public List<RelatorioImpostoDto> RelatorioImpostos { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int? UserId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "É necessário estar autenticado para aceder a esta página.";
                return RedirectToPage("/Login");
            }

            if (UserId == null)
            {
                TempData["ErrorMessage"] = "Utilizador não especificado.";
                return RedirectToPage("/Menu");
            }

            var user = await _context.Utilizadores.FindAsync(UserId.Value);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Utilizador não encontrado.";
                return RedirectToPage("/Menu");
            }

            var userRole = user.TipoUtilizador?.ToUpper() ?? "USER";

            var query = _context.PagamentoImpostos
                .Include(p => p.AtivoFinanceiro)
                .ThenInclude(a => a.Utilizador)
                .AsQueryable();

            if (userRole == "USER")
            {
                query = query.Where(p => p.AtivoFinanceiro.UtilizadorId == user.Id);
            }

            RelatorioImpostos = await query
                .Select(p => new RelatorioImpostoDto
                {
                    TipoAtivo = p.AtivoFinanceiro.Tipo,
                    DataPagamento = p.DataPagamento,
                    ValorImposto = p.Valor
                })
                .OrderByDescending(r => r.DataPagamento)
                .ToListAsync();

            return Page();
        }
    }
}