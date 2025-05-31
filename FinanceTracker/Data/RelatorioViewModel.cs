using System;
using System.Collections.Generic;

namespace FinanceTracker.Models
{
    public class RelatorioViewModel
    {
        public List<RelatorioAtivoDto> RelatorioAtivos { get; set; } = new();
        public List<RelatorioImpostoDto> RelatorioImpostos { get; set; } = new();
        public List<RelatorioBancoDto> RelatorioBancos { get; set; } = new();
    }

    public class RelatorioAtivoDto
    {
        public string TipoAtivo { get; set; }
        public DateTime DataInicio { get; set; }
        public decimal LucroTotalAntesImpostos { get; set; }
        public decimal LucroTotalAposImpostos { get; set; }
        public decimal LucroMensalMedioAntesImpostos { get; set; }
        public decimal LucroMensalMedioAposImpostos { get; set; }
    }

    public class RelatorioImpostoDto
    {
        public string TipoAtivo { get; set; }
        public DateTime DataPagamento { get; set; }
        public decimal ValorImposto { get; set; }
    }

    public class RelatorioBancoDto
    {
        public string Banco { get; set; }
        public decimal TotalDepositado { get; set; }
        public decimal JurosTotaisPagos { get; set; }
    }
}