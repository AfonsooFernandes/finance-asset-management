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

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Console.WriteLine("OnGetAsync iniciado.");
                
                var userId = Request.Query["userId"].ToString();
                Console.WriteLine($"UserId da query string: '{userId}'");

                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int utilizadorId))
                {
                    Console.WriteLine("Erro: UtilizadorId inválido ou não fornecido.");
                    return RedirectToPage("/Login");
                }
                Console.WriteLine($"UtilizadorId: {utilizadorId}");
                
                var utilizador = await _context.Utilizadores.FindAsync(utilizadorId);
                if (utilizador == null)
                {
                    Console.WriteLine($"Erro: Utilizador com ID {utilizadorId} não encontrado.");
                    return RedirectToPage("/Login");
                }
                Console.WriteLine($"Utilizador encontrado: {utilizador.Email}");
                
                try
                {
                    Console.WriteLine("Buscando AtivosFinanceiros...");
                    AtivosFinanceiros = await _context.AtivosFinanceiros
                        .Where(a => a.UtilizadorId == utilizadorId)
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
                    Console.WriteLine($"AtivosFinanceiros encontrados: {AtivosFinanceiros.Count} (IDs: {string.Join(", ", AtivosFinanceiros.Select(a => a.Id))})");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao buscar AtivosFinanceiros: {ex.Message}");
                    AtivosFinanceiros = new List<AtivoFinanceiroDto>();
                }

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