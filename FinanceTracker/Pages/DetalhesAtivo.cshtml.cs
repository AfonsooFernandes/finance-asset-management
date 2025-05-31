using FinanceTracker.Data;
using FinanceTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using System.Linq;

namespace FinanceTracker.Pages
{
    public class DetalhesAtivoModel : PageModel
    {
        private readonly FinanceTrackerContext _context;
        private readonly IAntiforgery _antiforgery;

        public DetalhesAtivoModel(FinanceTrackerContext context, IAntiforgery antiforgery)
        {
            _context = context;
            _antiforgery = antiforgery;
        }

        public AtivoFinanceiroDto AtivoFinanceiro { get; set; }
        public ImovelArrendadoDto ImovelArrendado { get; set; }
        public DepositoPrazoDto DepositoPrazo { get; set; }
        public FundoInvestimentoDto FundoInvestimento { get; set; }
        public string ErrorMessage { get; set; }

        private static string NormalizeTipo(string tipo)
        {
            if (string.IsNullOrEmpty(tipo)) return tipo;
            var normalized = tipo.ToLower().Replace(" ", "").Replace("á", "a").Replace("à", "a").Replace("ã", "a").Replace("â", "a");
            return normalized switch
            {
                "depósitoaprazo" => "depositoprazo",
                "fundodeinvestimento" => "fundoinvestimento",
                "imóvelarrendado" => "imovelarrendado",
                _ => normalized
            };
        }

        public async Task<IActionResult> OnGetAsync(string tipo, int id, int userId)
        {
            try
            {
                var queryTipo = Request.Query["tipo"].ToString();
                var queryId = Request.Query["id"].ToString();
                var queryUserId = Request.Query["userId"].ToString();
                Console.WriteLine($"OnGetAsync iniciado. Parâmetros: Tipo: '{tipo}', Id: {id}, UserId: {userId}");
                Console.WriteLine($"Query string: tipo='{queryTipo}', id='{queryId}', userId='{queryUserId}'");

                tipo = string.IsNullOrEmpty(tipo) ? queryTipo : tipo;
                id = id <= 0 && int.TryParse(queryId, out int parsedId) ? parsedId : id;
                userId = userId <= 0 && int.TryParse(queryUserId, out int parsedUserId) ? parsedUserId : userId;

                if (string.IsNullOrEmpty(tipo) || id <= 0 || userId <= 0)
                {
                    Console.WriteLine("Erro: Parâmetros inválidos.");
                    ErrorMessage = "Parâmetros inválidos.";
                    AtivoFinanceiro = new AtivoFinanceiroDto();
                    return Page();
                }

                var normalizedTipo = NormalizeTipo(tipo);
                Console.WriteLine($"Tipo normalizado: '{normalizedTipo}'");

                var utilizador = await _context.Utilizadores.FindAsync(userId);
                if (utilizador == null)
                {
                    Console.WriteLine($"Erro: Utilizador com ID {userId} não encontrado.");
                    ErrorMessage = "Utilizador não encontrado.";
                    AtivoFinanceiro = new AtivoFinanceiroDto();
                    return Page();
                }

                AtivoFinanceiro = await _context.AtivosFinanceiros
                    .Where(a => a.Id == id && a.UtilizadorId == userId)
                    .Select(a => new AtivoFinanceiroDto
                    {
                        Id = a.Id,
                        UtilizadorId = a.UtilizadorId,
                        Tipo = a.Tipo ?? "",
                        DataInicio = a.DataInicio.Date,
                        Duracao = a.Duracao,
                        Imposto = a.Imposto
                    })
                    .FirstOrDefaultAsync();

                if (AtivoFinanceiro == null)
                {
                    Console.WriteLine($"Erro: Ativo com ID {id} não encontrado.");
                    ErrorMessage = "Ativo não encontrado.";
                    AtivoFinanceiro = new AtivoFinanceiroDto { Id = id, UtilizadorId = userId, Tipo = tipo };
                    return Page();
                }

                switch (normalizedTipo)
                {
                    case "imovelarrendado":
                        ImovelArrendado = await _context.ImoveisArrendados
                            .Where(i => i.AtivoId == id)
                            .Select(i => new ImovelArrendadoDto
                            {
                                Id = i.Id,
                                AtivoId = i.AtivoId,
                                Designacao = i.Designacao ?? "",
                                Localizacao = i.Localizacao ?? "",
                                ValorImovel = double.IsNaN(i.ValorImovel) || double.IsInfinity(i.ValorImovel) ? 0f : Convert.ToSingle((double)i.ValorImovel),
                                ValorRenda = double.IsNaN(i.ValorRenda) || double.IsInfinity(i.ValorRenda) ? 0f : Convert.ToSingle((double)i.ValorRenda),
                                ValorCondominio = double.IsNaN(i.ValorCondominio) || double.IsInfinity(i.ValorCondominio) ? 0f : Convert.ToSingle((double)i.ValorCondominio),
                                OutrasDespesas = double.IsNaN(i.OutrasDespesas) || double.IsInfinity(i.OutrasDespesas) ? 0f : Convert.ToSingle((double)i.OutrasDespesas)
                            })
                            .FirstOrDefaultAsync();
                        if (ImovelArrendado == null)
                        {
                            Console.WriteLine($"Erro: Imóvel Arrendado com AtivoId {id} não encontrado.");
                            ErrorMessage = "Imóvel Arrendado não encontrado.";
                            return Page();
                        }
                        break;

                    case "depositoprazo":
                        DepositoPrazo = await _context.DepositosPrazo
                            .Where(d => d.AtivoId == id)
                            .Select(d => new DepositoPrazoDto
                            {
                                Id = d.Id,
                                AtivoId = d.AtivoId,
                                Valor = double.IsNaN(d.Valor) || double.IsInfinity(d.Valor) ? 0f : Convert.ToSingle((double)d.Valor),
                                Banco = d.Banco ?? "",
                                NumeroConta = d.NumeroConta ?? "",
                                Titulares = d.Titulares ?? "",
                                TaxaJuroAnual = double.IsNaN(d.TaxaJuroAnual) || double.IsInfinity(d.TaxaJuroAnual) ? 0f : Convert.ToSingle((double)d.TaxaJuroAnual)
                            })
                            .FirstOrDefaultAsync();
                        if (DepositoPrazo == null)
                        {
                            Console.WriteLine($"Erro: Depósito a Prazo com AtivoId {id} não encontrado.");
                            ErrorMessage = "Depósito a Prazo não encontrado.";
                            return Page();
                        }
                        break;

                    case "fundoinvestimento":
                        FundoInvestimento = await _context.FundosInvestimento
                            .Where(f => f.AtivoId == id)
                            .Select(f => new FundoInvestimentoDto
                            {
                                Id = f.Id,
                                AtivoId = f.AtivoId,
                                Nome = f.Nome ?? "",
                                Montante = double.IsNaN(f.Montante) || double.IsInfinity(f.Montante) ? 0f : Convert.ToSingle((double)f.Montante),
                                TaxaJuro = double.IsNaN(f.TaxaJuro) || double.IsInfinity(f.TaxaJuro) ? 0f : Convert.ToSingle((double)f.TaxaJuro)
                            })
                            .FirstOrDefaultAsync();
                        if (FundoInvestimento == null)
                        {
                            Console.WriteLine($"Erro: Fundo de Investimento com AtivoId {id} não encontrado.");
                            ErrorMessage = "Fundo de Investimento não encontrado.";
                            return Page();
                        }
                        break;

                    default:
                        Console.WriteLine($"Erro: Tipo de ativo '{normalizedTipo}' desconhecido.");
                        ErrorMessage = $"Tipo de ativo '{tipo}' desconhecido.";
                        return Page();
                }

                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro em OnGetAsync: {ex.Message}\nStackTrace: {ex.StackTrace}");
                ErrorMessage = "Erro interno no servidor: " + ex.Message;
                AtivoFinanceiro = new AtivoFinanceiroDto();
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(string tipo, int id, int userId)
        {
            try
            {
                Console.WriteLine($"OnPostDeleteAsync iniciado. Parâmetros: Tipo: '{tipo}', Id: {id}, UserId: {userId}");
                Console.WriteLine($"Form data: {string.Join(", ", HttpContext.Request.Form.Select(kvp => $"{kvp.Key}={kvp.Value}"))}");

                // Validar token antiforgery
                await _antiforgery.ValidateRequestAsync(HttpContext);
                Console.WriteLine("Token antiforgery validado.");

                if (string.IsNullOrEmpty(tipo) || id <= 0 || userId <= 0)
                {
                    Console.WriteLine("Erro: Parâmetros inválidos.");
                    ErrorMessage = "Parâmetros inválidos.";
                    AtivoFinanceiro = new AtivoFinanceiroDto { Id = id, UtilizadorId = userId, Tipo = tipo };
                    return Page();
                }

                var normalizedTipo = NormalizeTipo(tipo);
                Console.WriteLine($"Tipo normalizado: '{normalizedTipo}'");

                var ativo = await _context.AtivosFinanceiros
                    .Where(a => a.Id == id && a.UtilizadorId == userId)
                    .FirstOrDefaultAsync();

                if (ativo == null)
                {
                    Console.WriteLine($"Erro: Ativo com ID {id} não encontrado.");
                    ErrorMessage = "Ativo não encontrado.";
                    AtivoFinanceiro = new AtivoFinanceiroDto { Id = id, UtilizadorId = userId, Tipo = tipo };
                    return Page();
                }

                switch (normalizedTipo)
                {
                    case "imovelarrendado":
                        var imovel = await _context.ImoveisArrendados
                            .Where(i => i.AtivoId == id)
                            .FirstOrDefaultAsync();
                        if (imovel != null)
                        {
                            _context.ImoveisArrendados.Remove(imovel);
                            Console.WriteLine($"Imóvel Arrendado removido: AtivoId {id}");
                        }
                        break;

                    case "depositoprazo":
                        var deposito = await _context.DepositosPrazo
                            .Where(d => d.AtivoId == id)
                            .FirstOrDefaultAsync();
                        if (deposito != null)
                        {
                            _context.DepositosPrazo.Remove(deposito);
                            Console.WriteLine($"Depósito a Prazo removido: AtivoId {id}");
                        }
                        break;

                    case "fundoinvestimento":
                        var fundo = await _context.FundosInvestimento
                            .Where(f => f.AtivoId == id)
                            .FirstOrDefaultAsync();
                        if (fundo != null)
                        {
                            _context.FundosInvestimento.Remove(fundo);
                            Console.WriteLine($"Fundo de Investimento removido: AtivoId {id}");
                        }
                        break;

                    default:
                        Console.WriteLine($"Erro: Tipo de ativo '{normalizedTipo}' desconhecido.");
                        ErrorMessage = $"Tipo de ativo '{tipo}' desconhecido.";
                        AtivoFinanceiro = new AtivoFinanceiroDto { Id = id, UtilizadorId = userId, Tipo = tipo };
                        return Page();
                }

                _context.AtivosFinanceiros.Remove(ativo);
                await _context.SaveChangesAsync();
                Console.WriteLine($"AtivoFinanceiro removido: ID {id}, Tipo {tipo}");
                
                return RedirectToPage("/Index");
            }
            catch (AntiforgeryValidationException ex)
            {
                Console.WriteLine($"Erro de antiforgery em OnPostDeleteAsync: {ex.Message}\nStackTrace: {ex.StackTrace}");
                ErrorMessage = "Erro de validação antiforgery. Tente novamente.";
                AtivoFinanceiro = new AtivoFinanceiroDto { Id = id, UtilizadorId = userId, Tipo = tipo };
                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro em OnPostDeleteAsync: {ex.Message}\nStackTrace: {ex.StackTrace}");
                ErrorMessage = "Erro ao eliminar o ativo: " + ex.Message;
                AtivoFinanceiro = new AtivoFinanceiroDto { Id = id, UtilizadorId = userId, Tipo = tipo };
                return Page();
            }
        }
    }
}