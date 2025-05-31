using FinanceTracker.Models;
using FinanceTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceTracker.Pages
{
    public class RelatorioAtivosModel : PageModel
    {
        private readonly AtivoFinanceiroService _ativoFinanceiroService;
        private readonly ImovelArrendadoService _imovelArrendadoService;
        private readonly DepositoPrazoService _depositoPrazoService;
        private readonly FundoInvestimentoService _fundoInvestimentoService;

        public RelatorioAtivosModel(
            AtivoFinanceiroService ativoFinanceiroService,
            ImovelArrendadoService imovelArrendadoService,
            DepositoPrazoService depositoPrazoService,
            FundoInvestimentoService fundoInvestimentoService)
        {
            _ativoFinanceiroService = ativoFinanceiroService;
            _imovelArrendadoService = imovelArrendadoService;
            _depositoPrazoService = depositoPrazoService;
            _fundoInvestimentoService = fundoInvestimentoService;
        }

        public List<RelatorioAtivoDto> RelatorioAtivos { get; set; } = new List<RelatorioAtivoDto>();
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? userId)
        {
            Console.WriteLine($"[RelatorioAtivos] Iniciando OnGetAsync com userId: {(userId.HasValue ? userId.ToString() : "null")}");

            if (!userId.HasValue || userId <= 0)
            {
                ErrorMessage = "Utilizador inválido. Por favor, forneça um ID de utilizador válido.";
                Console.WriteLine($"[RelatorioAtivos] Erro: {ErrorMessage}");
                return Page();
            }

            var ativos = await _ativoFinanceiroService.GetAtivoById(userId.Value);
            if (ativos == null)
            {
                ErrorMessage = "Erro ao buscar ativos.";
                Console.WriteLine($"[RelatorioAtivos] Erro: {ErrorMessage}");
                return Page();
            }

            var ativosLista = ativos as IEnumerable<AtivoFinanceiroDto> ?? new List<AtivoFinanceiroDto> { ativos as AtivoFinanceiroDto }.Where(a => a != null);
            if (!ativosLista.Any())
            {
                ErrorMessage = "Nenhum ativo encontrado para este utilizador.";
                Console.WriteLine($"[RelatorioAtivos] Erro: {ErrorMessage}");
                return Page();
            }

            Console.WriteLine($"[RelatorioAtivos] Encontrados {ativosLista.Count()} ativos para userId: {userId}");

            var hoje = DateTime.Today; // 31/05/2025

            foreach (var ativo in ativosLista)
            {
                var relatorioItem = new RelatorioAtivoDto
                {
                    TipoAtivo = ativo.Tipo,
                    DataInicio = ativo.DataInicio
                };

                var meses = (hoje.Year - ativo.DataInicio.Year) * 12 + hoje.Month - ativo.DataInicio.Month;
                if (ativo.Duracao > 0 && meses > ativo.Duracao)
                {
                    meses = ativo.Duracao;
                }
                meses = Math.Max(1, meses);

                if (ativo.Tipo == "Imóvel Arrendado")
                {
                    var imovel = await _imovelArrendadoService.GetImovelById(ativo.Id);
                    if (imovel != null)
                    {
                        var rendaLiquida = imovel.ValorRenda - imovel.ValorCondominio - imovel.OutrasDespesas;
                        if (rendaLiquida > 0)
                        {
                            relatorioItem.LucroTotalAntesImpostos = (decimal)(rendaLiquida * meses);
                            relatorioItem.LucroTotalAposImpostos = relatorioItem.LucroTotalAntesImpostos * (decimal)(1 - ativo.Imposto / 100);
                            relatorioItem.LucroMensalMedioAntesImpostos = (decimal)(relatorioItem.LucroTotalAntesImpostos / meses);
                            relatorioItem.LucroMensalMedioAposImpostos = (decimal)(relatorioItem.LucroTotalAposImpostos / meses);
                        }
                    }
                }
                else if (ativo.Tipo == "Depósito a Prazo")
                {
                    var deposito = await _depositoPrazoService.GetDepositoById(ativo.Id);
                    if (deposito != null)
                    {
                        var jurosAnuais = deposito.Valor * deposito.TaxaJuroAnual / 100;
                        var anos = meses / 12.0f;
                        if (jurosAnuais > 0)
                        {
                            relatorioItem.LucroTotalAntesImpostos = (decimal)(jurosAnuais * anos);
                            relatorioItem.LucroTotalAposImpostos = relatorioItem.LucroTotalAntesImpostos * (decimal)(1 - ativo.Imposto / 100);
                            relatorioItem.LucroMensalMedioAntesImpostos = (decimal)(relatorioItem.LucroTotalAntesImpostos / meses);
                            relatorioItem.LucroMensalMedioAposImpostos = (decimal)(relatorioItem.LucroTotalAposImpostos / meses);
                        }
                    }
                }
                else if (ativo.Tipo == "Fundo de Investimento")
                {
                    var fundo = await _fundoInvestimentoService.GetFundoById(ativo.Id);
                    if (fundo != null)
                    {
                        var jurosAnuais = fundo.Montante * fundo.TaxaJuro / 100;
                        var anos = meses / 12.0f;
                        if (jurosAnuais > 0)
                        {
                            relatorioItem.LucroTotalAntesImpostos = (decimal)(jurosAnuais * anos);
                            relatorioItem.LucroTotalAposImpostos = relatorioItem.LucroTotalAntesImpostos * (decimal)(1 - ativo.Imposto / 100);
                            relatorioItem.LucroMensalMedioAntesImpostos = (decimal)(relatorioItem.LucroTotalAntesImpostos / meses);
                            relatorioItem.LucroMensalMedioAposImpostos = (decimal)(relatorioItem.LucroTotalAposImpostos / meses);
                        }
                    }
                }

                RelatorioAtivos.Add(relatorioItem);
                Console.WriteLine($"[RelatorioAtivos] Ativo processado: {ativo.Tipo}, Lucro Antes: {relatorioItem.LucroTotalAntesImpostos}");
            }

            Console.WriteLine($"[RelatorioAtivos] Finalizado com {RelatorioAtivos.Count} ativos processados.");
            return Page();
        }
    }
}