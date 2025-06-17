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
    public class ListarAtivosModel : PageModel
    {
        private readonly AtivoFinanceiroService _ativoFinanceiroService;
        private readonly ImovelArrendadoService _imovelArrendadoService;
        private readonly DepositoPrazoService _depositoPrazoService;
        private readonly FundoInvestimentoService _fundoInvestimentoService;

        public ListarAtivosModel(
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

        public List<AtivoFinanceiroDto> AtivosFinanceiros { get; set; } = new();

        public List<string> TiposDisponiveis { get; set; } = new List<string>
        {
            "Imóvel Arrendado",
            "Depósito a Prazo",
            "Fundo de Investimento"
        };

        [BindProperty(SupportsGet = true)]
        public int UserId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? TipoFiltro { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? IntervaloValor { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (UserId <= 0)
            {
                return RedirectToPage("/Login");
            }

            var ativos = (List<AtivoFinanceiroDto>)await _ativoFinanceiroService.GetAtivosByUserId(UserId);

            if (ativos == null || !ativos.Any())
            {
                AtivosFinanceiros = new List<AtivoFinanceiroDto>();
                return Page();
            }

            if (!string.IsNullOrEmpty(TipoFiltro))
            {
                ativos = ativos.Where(a => a.Tipo == TipoFiltro).ToList();
            }

            var listaOrdenada = new List<(AtivoFinanceiroDto Ativo, decimal ValorParaOrdenar)>();

            foreach (var ativo in ativos)
            {
                decimal valorParaOrdenar = ObterValor(ativo);

                listaOrdenada.Add((ativo, valorParaOrdenar));
            }

            // Aplicar filtro de valor (brackets)
            if (!string.IsNullOrEmpty(IntervaloValor))
            {
                decimal min = 0, max = decimal.MaxValue;

                switch (IntervaloValor)
                {
                    case "0-1000":
                        min = 0;
                        max = 1000;
                        break;
                    case "1000-10000":
                        min = 1000;
                        max = 10000;
                        break;
                    case "10000-100000":
                        min = 10000;
                        max = 100000;
                        break;
                    case "100000+":
                        min = 100000;
                        max = decimal.MaxValue;
                        break;
                }

                listaOrdenada = listaOrdenada
                    .Where(x => x.ValorParaOrdenar >= min && x.ValorParaOrdenar < max)
                    .ToList();
            }

            AtivosFinanceiros = listaOrdenada
                .OrderByDescending(x => x.ValorParaOrdenar)
                .Select(x => x.Ativo)
                .ToList();

            return Page();
        }

        public decimal ObterValor(AtivoFinanceiroDto ativo)
        {
            return ativo.Tipo switch
            {
                "Imóvel Arrendado" => (decimal)(_imovelArrendadoService.GetImovelByAtivoId(ativo.Id)?.Result?.ValorImovel ?? 0),
                "Depósito a Prazo" => (decimal)(_depositoPrazoService.GetDepositoByAtivoId(ativo.Id)?.Result?.Valor ?? 0),
                "Fundo de Investimento" => (decimal)(_fundoInvestimentoService.GetFundoByAtivoId(ativo.Id)?.Result?.Montante ?? 0),
                _ => 0m,
            };
        }
    }
}
