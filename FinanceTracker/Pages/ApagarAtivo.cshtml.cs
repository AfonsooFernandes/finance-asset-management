using FinanceTracker.Data;
using FinanceTracker.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FinanceTracker.Pages
{
    public class ApagarAtivoModel : PageModel
    {
        private readonly FinanceTrackerContext _context;
        private readonly IAntiforgery _antiforgery;

        public ApagarAtivoModel(FinanceTrackerContext context, IAntiforgery antiforgery)
        {
            _context = context;
            _antiforgery = antiforgery;
        }

        [BindProperty]
        public AtivoFinanceiroDto AtivoFinanceiro { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, int userId)
        {
            try
            {
                var ativo = await _context.AtivosFinanceiros
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.Id == id && a.UtilizadorId == userId);

                if (ativo == null)
                {
                    ErrorMessage = "Ativo financeiro não encontrado.";
                    return Page();
                }

                AtivoFinanceiro = new AtivoFinanceiroDto
                {
                    Id = ativo.Id,
                    UtilizadorId = ativo.UtilizadorId,
                    Tipo = ativo.Tipo
                };

                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erro ao carregar o ativo: {ex.Message}";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _antiforgery.ValidateRequestAsync(HttpContext);

                if (AtivoFinanceiro == null || AtivoFinanceiro.Id <= 0 || AtivoFinanceiro.UtilizadorId <= 0)
                {
                    ErrorMessage = "Parâmetros inválidos.";
                    return Page();
                }

                var ativo = await _context.AtivosFinanceiros
                    .FirstOrDefaultAsync(a => a.Id == AtivoFinanceiro.Id && a.UtilizadorId == AtivoFinanceiro.UtilizadorId);

                if (ativo == null)
                {
                    ErrorMessage = "Ativo financeiro não encontrado.";
                    return Page();
                }
                
                switch (ativo.Tipo.ToLower().Replace(" ", ""))
                {
                    case "depositoprazo":
                        var deposito = await _context.DepositosPrazo.FirstOrDefaultAsync(d => d.AtivoId == ativo.Id);
                        if (deposito != null)
                        {
                            _context.DepositosPrazo.Remove(deposito);
                        }
                        break;
                    case "fundoinvestimento":
                        var fundo = await _context.FundosInvestimento.FirstOrDefaultAsync(f => f.AtivoId == ativo.Id);
                        if (fundo != null)
                        {
                            _context.FundosInvestimento.Remove(fundo);
                        }
                        break;
                    case "imovelarrendado":
                        var imovel = await _context.ImoveisArrendados.FirstOrDefaultAsync(i => i.AtivoId == ativo.Id);
                        if (imovel != null)
                        {
                            _context.ImoveisArrendados.Remove(imovel);
                        }
                        break;
                }

                _context.AtivosFinanceiros.Remove(ativo);
                await _context.SaveChangesAsync();

                return RedirectToPage("/Listar_Ativos", new { userId = AtivoFinanceiro.UtilizadorId });
            }
            catch (AntiforgeryValidationException ex)
            {
                ErrorMessage = "Erro de validação antiforgery. Tente novamente.";
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erro ao apagar o ativo: {ex.Message}";
                return Page();
            }
        }
    }
}