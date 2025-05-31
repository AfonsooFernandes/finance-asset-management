namespace FinanceTracker.Models;

public class RelatorioImpostosDto
{
    public string TipoAtivo { get; set; }
    public DateTime DataPagamento { get; set; }
    public double ValorImposto { get; set; }
}