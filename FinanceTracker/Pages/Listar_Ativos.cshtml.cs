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
    public class ListarAtivosModel : PageModel
    {
        private readonly FinanceTrackerContext _context;

        public ListarAtivosModel(FinanceTrackerContext context)
        {
            _context = context;
        }

        public IList<AtivoFinanceiroDto> AtivosFinanceiros { get; set; } = new List<AtivoFinanceiroDto>();
        public List<string> TiposDisponiveis { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var userId = Request.Query["userId"].ToString();
                var tipoFiltro = Request.Query["tipo"].ToString()?.Trim();

                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int utilizadorId))
                    return RedirectToPage("/Login");

                var utilizador = await _context.Utilizadores.FindAsync(utilizadorId);
                if (utilizador == null)
                    return RedirectToPage("/Login");

                var query = _context.AtivosFinanceiros
                    .Where(a => a.UtilizadorId == utilizadorId);

                if (!string.IsNullOrEmpty(tipoFiltro))
                    query = query.Where(a => a.Tipo == tipoFiltro);

                AtivosFinanceiros = await query
                    .Select(a => new AtivoFinanceiroDto
                    {
                        Id = a.Id,
                        UtilizadorId = a.UtilizadorId,
                        Tipo = a.Tipo ?? "",
                        DataInicio = a.DataInicio,
                        Duracao = a.Duracao,
                        Imposto = a.Imposto
                    })
                    .ToListAsync();

                TiposDisponiveis = await _context.AtivosFinanceiros
                    .Where(a => a.UtilizadorId == utilizadorId && a.Tipo != null)
                    .Select(a => a.Tipo)
                    .Distinct()
                    .OrderBy(t => t)
                    .ToListAsync();

                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro geral em OnGetAsync: {ex.Message}");
                return StatusCode(500, "Erro interno no servidor.");
            }
        }
    }
}
