namespace FinanceTracker.Models;

public class RelatorioAtivosDto
{
    public string TipoAtivo { get; set; }
    public DateTime DataInicio { get; set; }
    public float LucroTotalAntesImpostos { get; set; }
    public float LucroTotalAposImpostos { get; set; }
    public float LucroMensalMedioAntesImpostos { get; set; }
    public float LucroMensalMedioAposImpostos { get; set; }
}
