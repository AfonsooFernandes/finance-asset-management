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

    public List<RelatorioAtivoDto> RelatorioAtivos { get; set; } = new();
    public string ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int userId)
    {
        var ativos = (List<AtivoFinanceiroDto>)await _ativoFinanceiroService.GetAtivosByUserId(userId); // <- Aqui assume que retorna uma lista
        if (ativos == null || !ativos.Any())
        {
            ErrorMessage = "Nenhum ativo encontrado para este utilizador.";
            return Page();
        }
        
        var hoje = DateTime.Today;

        foreach (var ativo in ativos)
        {
            var relatorio = new RelatorioAtivoDto
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
                var imovel = await _imovelArrendadoService.GetImovelByAtivoId(ativo.Id);
                if (imovel != null)
                {
                    var rendaLiquida = imovel.ValorRenda - imovel.ValorCondominio - imovel.OutrasDespesas;
                    relatorio.LucroTotalAntesImpostos = (decimal)(rendaLiquida * meses);
                }
            }
            else if (ativo.Tipo == "Depósito a Prazo")
            {
                var deposito = await _depositoPrazoService.GetDepositoByAtivoId(ativo.Id);
                if (deposito != null)
                {
                    var jurosAnuais = deposito.Valor * deposito.TaxaJuroAnual / 100;
                    relatorio.LucroTotalAntesImpostos = (decimal)(jurosAnuais * (meses / 12.0));
                }
            }
            else if (ativo.Tipo == "Fundo de Investimento")
            {
                var fundo = await _fundoInvestimentoService.GetFundoByAtivoId(ativo.Id);
                if (fundo != null)
                {
                    var jurosAnuais = fundo.Montante * fundo.TaxaJuro / 100;
                    relatorio.LucroTotalAntesImpostos = (decimal)(jurosAnuais * (meses / 12.0));
                }
            }

            // Cálculo de impostos
            relatorio.LucroTotalAposImpostos = relatorio.LucroTotalAntesImpostos * (decimal)(1 - ativo.Imposto / 100);
            relatorio.LucroMensalMedioAntesImpostos = relatorio.LucroTotalAntesImpostos / meses;
            relatorio.LucroMensalMedioAposImpostos = relatorio.LucroTotalAposImpostos / meses;

            RelatorioAtivos.Add(relatorio);
        }

        return Page();
    }
}
}
