using FinanceTracker.Models;

namespace FinanceTracker.Data
{
    public class ImovelArrendado
    {
        public int Id { get; set; }
        public int AtivoId { get; set; }
        public string Designacao { get; set; }
        public string Localizacao { get; set; }
        public double ValorImovel { get; set; }
        public double ValorRenda { get; set; }
        public double ValorCondominio { get; set; }
        public double OutrasDespesas { get; set; }

        public AtivoFinanceiro AtivoFinanceiro { get; set; }
        
    }
}