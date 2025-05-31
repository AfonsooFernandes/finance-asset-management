using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinanceTracker.Data;
using FinanceTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceTracker.Pages
{
    public class RelatorioAtivosModel : PageModel
    {
        private readonly FinanceTrackerContext _context;

        public RelatorioAtivosModel(FinanceTrackerContext context)
        {
            _context = context;
        }

        public List<RelatorioAtivoDto> RelatorioAtivos { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            RelatorioAtivos = await _context.AtivosFinanceiros
                .Include(a => a.Utilizador)
                .Select(a => new RelatorioAtivoDto
                {
                    TipoAtivo = a.Tipo,
                    DataInicio = a.DataInicio,
                    LucroTotalAntesImpostos = a.LucroTotalAntesImpostos,
                    LucroTotalAposImpostos = a.LucroTotalAposImpostos,
                    LucroMensalMedioAntesImpostos = a.LucroMensalMedioAntesImpostos,
                    LucroMensalMedioAposImpostos = a.LucroMensalMedioAposImpostos
                })
                .ToListAsync();

            return Page();
        }
    }

    public class RelatorioAtivoDto
    {
        public string TipoAtivo { get; set; }
        public DateTime DataInicio { get; set; }
        public decimal LucroTotalAntesImpostos { get; set; }
        public decimal LucroTotalAposImpostos { get; set; }
        public decimal LucroMensalMedioAntesImpostos { get; set; }
        public decimal LucroMensalMedioAposImpostos { get; set; }
    }
}