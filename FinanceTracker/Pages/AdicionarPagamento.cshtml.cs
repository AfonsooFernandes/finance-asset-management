using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinanceTracker.Data;
using FinanceTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceTracker.Pages
{
    public class AdicionarPagamentoModel : PageModel
    {
        private readonly FinanceTrackerContext _context;

        public AdicionarPagamentoModel(FinanceTrackerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PagamentoImpostos Pagamento { get; set; }

        public List<AtivoFinanceiro> Ativos { get; set; }

        public async Task OnGetAsync()
        {
            Ativos = await _context.AtivosFinanceiros 
                .OrderBy(a => a.Id)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.PagamentoImpostos.Add(Pagamento);
            await _context.SaveChangesAsync();

            return RedirectToPage("/RelatorioImpostos");
        }
    }
}