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
    public class EditarAtivoModel : PageModel
    {
        private readonly FinanceTrackerContext _context;
        private readonly IAntiforgery _antiforgery;

        public EditarAtivoModel(FinanceTrackerContext context, IAntiforgery antiforgery)
        {
            _context = context;
            _antiforgery = antiforgery;
        }

        [BindProperty]
        public AtivoFinanceiroDto AtivoFinanceiro { get; set; }
        [BindProperty]
        public DepositoPrazoDto DepositoPrazo { get; set; }
        [BindProperty]
        public FundoInvestimentoDto FundoInvestimento { get; set; }
        [BindProperty]
        public ImovelArrendadoDto ImovelArrendado { get; set; }
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
                Console.WriteLine($"OnGetAsync iniciado. Parâmetros do método: Tipo: '{tipo}', Id: {id}, UserId: {userId}");
                Console.WriteLine($"Query string: tipo='{queryTipo}', id='{queryId}', userId='{queryUserId}'");

                tipo = string.IsNullOrEmpty(tipo) ? queryTipo : tipo;
                id = id <= 0 && int.TryParse(queryId, out int parsedId) ? parsedId : id;
                userId = userId <= 0 && int.TryParse(queryUserId, out int parsedUserId) ? parsedUserId : userId;

                if (string.IsNullOrEmpty(tipo) || id <= 0 || userId <= 0)
                {
                    Console.WriteLine("Erro: Parâmetros inválidos após verificação da query string.");
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
                Console.WriteLine($"Utilizador encontrado: {utilizador.Email}");

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
                    Console.WriteLine($"Erro: Ativo com ID {id} não encontrado para o utilizador {userId}.");
                    ErrorMessage = "Ativo não encontrado.";
                    AtivoFinanceiro = new AtivoFinanceiroDto { Id = id, UtilizadorId = userId, Tipo = tipo };
                    return Page();
                }
                Console.WriteLine($"AtivoFinanceiro encontrado: ID {AtivoFinanceiro.Id}, Tipo {AtivoFinanceiro.Tipo}");

                if (!string.Equals(normalizedTipo, NormalizeTipo(AtivoFinanceiro.Tipo), StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Erro: Tipo normalizado '{normalizedTipo}' não corresponde ao tipo do ativo '{AtivoFinanceiro.Tipo}' (normalizado: '{NormalizeTipo(AtivoFinanceiro.Tipo)}').");
                    ErrorMessage = "Tipo de ativo inválido.";
                    return Page();
                }

                switch (normalizedTipo)
                {
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
                        Console.WriteLine($"Depósito a Prazo carregado: AtivoId {DepositoPrazo.AtivoId}, Valor {DepositoPrazo.Valor}");
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
                        Console.WriteLine($"Fundo de Investimento carregado: AtivoId {FundoInvestimento.AtivoId}, Montante {FundoInvestimento.Montante}");
                        break;

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
                        Console.WriteLine($"Imóvel Arrendado carregado: AtivoId {ImovelArrendado.AtivoId}, Designacao {ImovelArrendado.Designacao}");
                        break;

                    default:
                        Console.WriteLine($"Erro: Tipo de ativo normalizado '{normalizedTipo}' desconhecido.");
                        ErrorMessage = $"Tipo de ativo '{tipo}' desconhecido.";
                        return Page();
                }

                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro geral em OnGetAsync: {ex.Message}\nStackTrace: {ex.StackTrace}");
                ErrorMessage = "Erro interno no servidor: " + ex.Message;
                AtivoFinanceiro = new AtivoFinanceiroDto();
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(string tipo)
        {
            try
            {
                Console.WriteLine($"OnPostAsync iniciado. AtivoFinanceiro ID: {AtivoFinanceiro?.Id}, Tipo: {AtivoFinanceiro?.Tipo}, Query Tipo: {tipo}");
                Console.WriteLine($"Cookies recebidos: {string.Join(", ", HttpContext.Request.Cookies.Keys)}");
                Console.WriteLine($"Form data: {string.Join(", ", HttpContext.Request.Form.Select(kvp => $"{kvp.Key}={kvp.Value}"))}");

                // Debug: Validar token antiforgery manualmente
                await _antiforgery.ValidateRequestAsync(HttpContext);
                Console.WriteLine("Token antiforgery validado manualmente.");

                if (AtivoFinanceiro == null)
                {
                    Console.WriteLine("Erro: AtivoFinanceiro é nulo.");
                    ErrorMessage = "Dados do ativo financeiro não fornecidos.";
                    return Page();
                }

                // Usar tipo da query string como fallback se AtivoFinanceiro.Tipo estiver vazio
                var effectiveTipo = string.IsNullOrEmpty(AtivoFinanceiro.Tipo) ? tipo : AtivoFinanceiro.Tipo;
                if (string.IsNullOrEmpty(effectiveTipo))
                {
                    Console.WriteLine("Erro: Tipo do ativo não fornecido no formulário ou na query string.");
                    ErrorMessage = "Tipo do ativo não fornecido.";
                    return Page();
                }

                var normalizedTipo = NormalizeTipo(effectiveTipo);
                Console.WriteLine($"Tipo normalizado: '{normalizedTipo}'");

                // Limpar todos os erros do ModelState
                ModelState.Clear();
                Console.WriteLine("ModelState limpo.");

                // Revalidar apenas os modelos relevantes
                TryValidateModel(AtivoFinanceiro, nameof(AtivoFinanceiro));
                if (normalizedTipo == "imovelarrendado" && ImovelArrendado != null)
                {
                    TryValidateModel(ImovelArrendado, nameof(ImovelArrendado));
                }
                else if (normalizedTipo == "depositoprazo" && DepositoPrazo != null)
                {
                    TryValidateModel(DepositoPrazo, nameof(DepositoPrazo));
                }
                else if (normalizedTipo == "fundoinvestimento" && FundoInvestimento != null)
                {
                    TryValidateModel(FundoInvestimento, nameof(FundoInvestimento));
                }

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.SelectMany(m => m.Value.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    Console.WriteLine($"Erro: Modelo inválido. Erros: {string.Join("; ", errors)}");
                    ErrorMessage = "Dados inválidos: " + string.Join("; ", errors);
                    return Page();
                }

                var ativo = await _context.AtivosFinanceiros
                    .Where(a => a.Id == AtivoFinanceiro.Id && a.UtilizadorId == AtivoFinanceiro.UtilizadorId)
                    .FirstOrDefaultAsync();

                if (ativo == null)
                {
                    Console.WriteLine($"Erro: Ativo com ID {AtivoFinanceiro.Id} não encontrado.");
                    ErrorMessage = "Ativo não encontrado.";
                    return Page();
                }

                ativo.DataInicio = AtivoFinanceiro.DataInicio;
                ativo.Duracao = AtivoFinanceiro.Duracao;
                ativo.Imposto = AtivoFinanceiro.Imposto;

                switch (normalizedTipo)
                {
                    case "depositoprazo":
                        if (DepositoPrazo == null)
                        {
                            Console.WriteLine("Erro: Dados de Depósito a Prazo não fornecidos.");
                            ErrorMessage = "Dados de Depósito a Prazo não fornecidos.";
                            return Page();
                        }
                        // Ler valores como double
                        var depositoRaw = await _context.DepositosPrazo
                            .Where(d => d.AtivoId == AtivoFinanceiro.Id)
                            .Select(d => new
                            {
                                d.Id,
                                d.AtivoId,
                                d.Banco,
                                d.NumeroConta,
                                d.Titulares,
                                Valor = (double)d.Valor,
                                TaxaJuroAnual = (double)d.TaxaJuroAnual
                            })
                            .FirstOrDefaultAsync();
                        if (depositoRaw == null)
                        {
                            Console.WriteLine($"Erro: Depósito a Prazo com AtivoId {AtivoFinanceiro.Id} não encontrado.");
                            ErrorMessage = "Depósito a Prazo não encontrado.";
                            return Page();
                        }
                        Console.WriteLine($"Depósito a Prazo lido do banco: AtivoId {depositoRaw.AtivoId}, Valor {depositoRaw.Valor}, TaxaJuroAnual {depositoRaw.TaxaJuroAnual}");

                        // Criar entidade para atualização
                        var deposito = new DepositoPrazo
                        {
                            Id = depositoRaw.Id,
                            AtivoId = depositoRaw.AtivoId,
                            Banco = DepositoPrazo.Banco,
                            NumeroConta = DepositoPrazo.NumeroConta,
                            Titulares = DepositoPrazo.Titulares,
                            Valor = DepositoPrazo.Valor,
                            TaxaJuroAnual = DepositoPrazo.TaxaJuroAnual
                        };

                        try
                        {
                            // Validar conversões para float
                            deposito.Valor = Convert.ToSingle(deposito.Valor);
                            deposito.TaxaJuroAnual = Convert.ToSingle(deposito.TaxaJuroAnual);
                        }
                        catch (OverflowException ex)
                        {
                            Console.WriteLine($"Erro de conversão para float em Depósito a Prazo: {ex.Message}");
                            ErrorMessage = "Os valores numéricos são muito grandes para serem salvos.";
                            return Page();
                        }

                        // Anexar e marcar como modificado
                        _context.DepositosPrazo.Attach(deposito);
                        _context.Entry(deposito).Property(d => d.Banco).IsModified = true;
                        _context.Entry(deposito).Property(d => d.NumeroConta).IsModified = true;
                        _context.Entry(deposito).Property(d => d.Titulares).IsModified = true;
                        _context.Entry(deposito).Property(d => d.Valor).IsModified = true;
                        _context.Entry(deposito).Property(d => d.TaxaJuroAnual).IsModified = true;

                        Console.WriteLine($"Depósito a Prazo preparado para atualização: AtivoId {deposito.AtivoId}, Valor {deposito.Valor}, TaxaJuroAnual {deposito.TaxaJuroAnual}");
                        break;

                    case "fundoinvestimento":
                        if (FundoInvestimento == null)
                        {
                            Console.WriteLine("Erro: Dados de Fundo de Investimento não fornecidos.");
                            ErrorMessage = "Dados de Fundo de Investimento não fornecidos.";
                            return Page();
                        }
                        // Ler valores como double
                        var fundoRaw = await _context.FundosInvestimento
                            .Where(f => f.AtivoId == AtivoFinanceiro.Id)
                            .Select(f => new
                            {
                                f.Id,
                                f.AtivoId,
                                f.Nome,
                                Montante = (double)f.Montante,
                                TaxaJuro = (double)f.TaxaJuro
                            })
                            .FirstOrDefaultAsync();
                        if (fundoRaw == null)
                        {
                            Console.WriteLine($"Erro: Fundo de Investimento com AtivoId {AtivoFinanceiro.Id} não encontrado.");
                            ErrorMessage = "Fundo de Investimento não encontrado.";
                            return Page();
                        }
                        Console.WriteLine($"Fundo de Investimento lido do banco: AtivoId {fundoRaw.AtivoId}, Montante {fundoRaw.Montante}, TaxaJuro {fundoRaw.TaxaJuro}");

                        // Criar entidade para atualização
                        var fundo = new FundoInvestimento
                        {
                            Id = fundoRaw.Id,
                            AtivoId = fundoRaw.AtivoId,
                            Nome = FundoInvestimento.Nome,
                            Montante = FundoInvestimento.Montante,
                            TaxaJuro = FundoInvestimento.TaxaJuro
                        };

                        try
                        {
                            // Validar conversões para float
                            fundo.Montante = Convert.ToSingle(fundo.Montante);
                            fundo.TaxaJuro = Convert.ToSingle(fundo.TaxaJuro);
                        }
                        catch (OverflowException ex)
                        {
                            Console.WriteLine($"Erro de conversão para float em Fundo de Investimento: {ex.Message}");
                            ErrorMessage = "Os valores numéricos são muito grandes para serem salvos.";
                            return Page();
                        }

                        // Anexar e marcar como modificado
                        _context.FundosInvestimento.Attach(fundo);
                        _context.Entry(fundo).Property(f => f.Nome).IsModified = true;
                        _context.Entry(fundo).Property(f => f.Montante).IsModified = true;
                        _context.Entry(fundo).Property(f => f.TaxaJuro).IsModified = true;

                        Console.WriteLine($"Fundo de Investimento preparado para atualização: AtivoId {fundo.AtivoId}, Montante {fundo.Montante}, TaxaJuro {fundo.TaxaJuro}");
                        break;

                    case "imovelarrendado":
                        if (ImovelArrendado == null)
                        {
                            Console.WriteLine("Erro: Dados de Imóvel Arrendado não fornecidos.");
                            ErrorMessage = "Dados de Imóvel Arrendado não fornecidos.";
                            return Page();
                        }
                        var imovelRaw = await _context.ImoveisArrendados
                            .Where(i => i.AtivoId == AtivoFinanceiro.Id)
                            .Select(i => new
                            {
                                i.Id,
                                i.AtivoId,
                                i.Designacao,
                                i.Localizacao,
                                ValorImovel = (double)i.ValorImovel,
                                ValorRenda = (double)i.ValorRenda,
                                ValorCondominio = (double)i.ValorCondominio,
                                OutrasDespesas = (double)i.OutrasDespesas
                            })
                            .FirstOrDefaultAsync();
                        if (imovelRaw == null)
                        {
                            Console.WriteLine($"Erro: Imóvel Arrendado com AtivoId {AtivoFinanceiro.Id} não encontrado.");
                            ErrorMessage = "Imóvel Arrendado não encontrado.";
                            return Page();
                        }
                        Console.WriteLine($"Imóvel Arrendado lido do banco: AtivoId {imovelRaw.AtivoId}, Designacao {imovelRaw.Designacao}, ValorImovel {imovelRaw.ValorImovel}, ValorRenda {imovelRaw.ValorRenda}, ValorCondominio {imovelRaw.ValorCondominio}, OutrasDespesas {imovelRaw.OutrasDespesas}");
                        
                        var imovel = new ImovelArrendado
                        {
                            Id = imovelRaw.Id,
                            AtivoId = imovelRaw.AtivoId,
                            Designacao = ImovelArrendado.Designacao,
                            Localizacao = ImovelArrendado.Localizacao,
                            ValorImovel = ImovelArrendado.ValorImovel,
                            ValorRenda = ImovelArrendado.ValorRenda,
                            ValorCondominio = ImovelArrendado.ValorCondominio,
                            OutrasDespesas = ImovelArrendado.OutrasDespesas
                        };

                        try
                        {
                            imovel.ValorImovel = Convert.ToSingle(imovel.ValorImovel);
                            imovel.ValorRenda = Convert.ToSingle(imovel.ValorRenda);
                            imovel.ValorCondominio = Convert.ToSingle(imovel.ValorCondominio);
                            imovel.OutrasDespesas = Convert.ToSingle(imovel.OutrasDespesas);
                        }
                        catch (OverflowException ex)
                        {
                            Console.WriteLine($"Erro de conversão para float: {ex.Message}");
                            ErrorMessage = "Os valores numéricos são muito grandes para serem salvos.";
                            return Page();
                        }
                        
                        _context.ImoveisArrendados.Attach(imovel);
                        _context.Entry(imovel).Property(i => i.Designacao).IsModified = true;
                        _context.Entry(imovel).Property(i => i.Localizacao).IsModified = true;
                        _context.Entry(imovel).Property(i => i.ValorImovel).IsModified = true;
                        _context.Entry(imovel).Property(i => i.ValorRenda).IsModified = true;
                        _context.Entry(imovel).Property(i => i.ValorCondominio).IsModified = true;
                        _context.Entry(imovel).Property(i => i.OutrasDespesas).IsModified = true;

                        Console.WriteLine($"Imóvel Arrendado preparado para atualização: AtivoId {imovel.AtivoId}, Designacao {imovel.Designacao}, ValorImovel {imovel.ValorImovel}, ValorRenda {imovel.ValorRenda}, ValorCondominio {imovel.ValorCondominio}, OutrasDespesas {imovel.OutrasDespesas}");
                        break;

                    default:
                        Console.WriteLine($"Erro: Tipo de ativo normalizado '{normalizedTipo}' desconhecido.");
                        ErrorMessage = $"Tipo de ativo '{effectiveTipo}' desconhecido.";
                        return Page();
                }

                await _context.SaveChangesAsync();
                Console.WriteLine($"AtivoFinanceiro atualizado: ID {ativo.Id}, Tipo {ativo.Tipo}");

                return RedirectToPage("/DetalhesAtivo", new { tipo = AtivoFinanceiro.Tipo, id = AtivoFinanceiro.Id, userId = AtivoFinanceiro.UtilizadorId });
            }
            catch (AntiforgeryValidationException ex)
            {
                Console.WriteLine($"Erro de antiforgery em OnPostAsync: {ex.Message}\nStackTrace: {ex.StackTrace}");
                ErrorMessage = "Erro de validação antiforgery. Tente novamente.";
                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro geral em OnPostAsync: {ex.Message}\nStackTrace: {ex.StackTrace}");
                ErrorMessage = "Erro ao salvar as alterações: " + ex.Message;
                return Page();
            }
        }
    }
}