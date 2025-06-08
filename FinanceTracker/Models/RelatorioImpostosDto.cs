namespace FinanceTracker.Models
{
    public class RelatorioImpostosDto
    {
        public string TipoAtivo { get; set; }
        public DateTime DataPagamento { get; set; }
        public decimal ValorImposto { get; set; }
    }
}