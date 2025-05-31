using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinanceTracker.Data;
using FinanceTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceTracker.Pages
{
    public class RelatorioImpostosModel : PageModel
    {
        private readonly FinanceTrackerContext _context;

        public RelatorioImpostosModel(FinanceTrackerContext context)
        {
            _context = context;
        }

        public List<RelatorioImpostoDto> RelatorioImpostos { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            RelatorioImpostos = await _context.PagamentoImpostos
                .Include(p => p.AtivoFinanceiro)
                .ThenInclude(a => a.Utilizador)
                .Select(p => new RelatorioImpostoDto
                {
                    TipoAtivo = p.AtivoFinanceiro.Tipo,
                    DataPagamento = p.DataPagamento,
                    ValorImposto = p.Valor
                })
                .ToListAsync();

            return Page();
        }
    }

    public class RelatorioImpostoDto
    {
        public string TipoAtivo { get; set; }
        public DateTime DataPagamento { get; set; }
        public decimal ValorImposto { get; set; }
    }
}