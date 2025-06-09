namespace FinanceTracker.Data
{
    public class JurosMensaisFundo
    {
        public int Id { get; set; }
        public int FundoId { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
        public double Taxa { get; set; }

        public FundoInvestimento FundoInvestimento { get; set; }
    }
}