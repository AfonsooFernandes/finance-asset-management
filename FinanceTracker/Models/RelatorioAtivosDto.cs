namespace FinanceTracker.Models;

public class RelatorioAtivosDto
{
    public string TipoAtivo { get; set; }
    public DateTime DataInicio { get; set; }
    public double LucroTotalAntesImpostos { get; set; }
    public double LucroTotalAposImpostos { get; set; }
    public double LucroMensalMedioAntesImpostos { get; set; }
    public double LucroMensalMedioAposImpostos { get; set; }
}
