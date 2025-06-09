using FinanceTracker.Data;
using FinanceTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
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

        public List<AtivoFinanceiroDto> Ativos { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            await CarregarAtivosAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await CarregarAtivosAsync();
                return Page();
            }

            var ativo = await _context.AtivosFinanceiros
                .Include(a => a.DepositoPrazo)
                .Include(a => a.FundoInvestimento)
                .Include(a => a.ImovelArrendado)
                .FirstOrDefaultAsync(a => a.Id == Pagamento.AtivoId);

            if (ativo == null)
            {
                ModelState.AddModelError("", "Ativo não encontrado.");
                await CarregarAtivosAsync();
                return Page();
            }

            decimal lucroAntesImpostos = ativo.DepositoPrazo != null
                ? (decimal)ativo.DepositoPrazo.Valor * 0.05m * ativo.Duracao
                : ativo.FundoInvestimento != null
                    ? (decimal)ativo.FundoInvestimento.Montante * 0.06m * ativo.Duracao
                    : ativo.ImovelArrendado != null
                        ? (decimal)ativo.ImovelArrendado.ValorRenda * ativo.Duracao
                        : 0m;

            Pagamento.Valor = lucroAntesImpostos * ((decimal)ativo.Imposto / 100);
            Pagamento.DataPagamento = DateTime.Now;

            _context.PagamentoImpostos.Add(Pagamento);
            await _context.SaveChangesAsync();
            
            return RedirectToPage("/RelatorioImpostos", new { userId = ativo.UtilizadorId });
        }

        private async Task CarregarAtivosAsync()
        {
            Ativos = await _context.AtivosFinanceiros
                .Include(a => a.DepositoPrazo)
                .Include(a => a.FundoInvestimento)
                .Include(a => a.ImovelArrendado)
                .OrderBy(a => a.Id)
                .Select(a => new AtivoFinanceiroDto
                {
                    Id = a.Id,
                    UtilizadorId = a.UtilizadorId,
                    Tipo = a.Tipo,
                    DataInicio = a.DataInicio,
                    Duracao = a.Duracao,
                    Imposto = a.Imposto,
                    LucroTotalAntesImpostos = a.DepositoPrazo != null
                        ? (decimal)a.DepositoPrazo.Valor * 0.05m * a.Duracao
                        : a.FundoInvestimento != null
                            ? (decimal)a.FundoInvestimento.Montante * 0.06m * a.Duracao
                            : a.ImovelArrendado != null
                                ? (decimal)a.ImovelArrendado.ValorRenda * a.Duracao
                                : 0m
                })
                .ToListAsync();
        }
    }
}