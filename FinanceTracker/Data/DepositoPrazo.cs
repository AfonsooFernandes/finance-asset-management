using FinanceTracker.Models;

namespace FinanceTracker.Data
{
    public class DepositoPrazo
    {
        public int Id { get; set; }
        public int AtivoId { get; set; }
        public double Valor { get; set; }
        public string Banco { get; set; }
        public string NumeroConta { get; set; }
        public string Titulares { get; set; }
        public double TaxaJuroAnual { get; set; }
        
        public AtivoFinanceiro AtivoFinanceiro { get; set; }
        
    }
}