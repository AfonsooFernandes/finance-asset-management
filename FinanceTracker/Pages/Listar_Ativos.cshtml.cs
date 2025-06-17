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
            // Adicione outros tipos se houver
        };

        [BindProperty(SupportsGet = true)]
        public int UserId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? TipoFiltro { get; set; }

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

            // Aplica filtro por tipo se fornecido
            if (!string.IsNullOrEmpty(TipoFiltro))
            {
                ativos = ativos.Where(a => a.Tipo == TipoFiltro).ToList();
            }

            var listaOrdenada = new List<(AtivoFinanceiroDto Ativo, decimal ValorParaOrdenar)>();

            foreach (var ativo in ativos)
            {
                decimal valorParaOrdenar = 0m;

                switch (ativo.Tipo)
                {
                    case "Imóvel Arrendado":
                        var imovel = await _imovelArrendadoService.GetImovelByAtivoId(ativo.Id);
                        if (imovel != null)
                        {
                            valorParaOrdenar = (decimal)imovel.ValorImovel; // CAST explícito
                        }
                        break;

                    case "Depósito a Prazo":
                        var deposito = await _depositoPrazoService.GetDepositoByAtivoId(ativo.Id);
                        if (deposito != null)
                        {
                            valorParaOrdenar = (decimal)deposito.Valor; // CAST explícito
                        }
                        break;

                    case "Fundo de Investimento":
                        var fundo = await _fundoInvestimentoService.GetFundoByAtivoId(ativo.Id);
                        if (fundo != null)
                        {
                            valorParaOrdenar = (decimal)fundo.Montante; // CAST explícito
                        }
                        break;

                    default:
                        valorParaOrdenar = 0m;
                        break;
                }

                listaOrdenada.Add((ativo, valorParaOrdenar));
            }

            // Ordenar decrescente pelo valor calculado
            AtivosFinanceiros = listaOrdenada
                .OrderByDescending(x => x.ValorParaOrdenar)
                .Select(x => x.Ativo)
                .ToList();

            return Page();
        }
    }
}
