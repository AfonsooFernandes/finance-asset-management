using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinanceTracker.Data;
using FinanceTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace FinanceTracker.Pages
{
    public class RelatorioImpostosModel : PageModel
    {
        private readonly FinanceTrackerContext _context;

        public RelatorioImpostosModel(FinanceTrackerContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int UserId { get; set; }

        public List<RelatorioImpostoDto> RelatorioImpostos { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (UserId <= 0)
            {
                ErrorMessage = "ID de utilizador inválido.";
                return Page();
            }

            try
            {
                RelatorioImpostos = await _context.PagamentoImpostos
                    .Include(p => p.AtivoFinanceiro)
                    .ThenInclude(a => a.Utilizador)
                    .Where(p => p.AtivoFinanceiro.UtilizadorId == UserId)
                    .OrderByDescending(p => p.DataPagamento)
                    .Select(p => new RelatorioImpostoDto
                    {
                        TipoAtivo = p.AtivoFinanceiro.Tipo,
                        DataPagamento = p.DataPagamento,
                        ValorImposto = p.Valor
                    })
                    .ToListAsync();

                if (RelatorioImpostos == null || !RelatorioImpostos.Any())
                {
                    ErrorMessage = "Nenhum pagamento de imposto foi encontrado.";
                }
            }
            catch
            {
                ErrorMessage = "Ocorreu um erro ao carregar o relatório de impostos.";
            }

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
