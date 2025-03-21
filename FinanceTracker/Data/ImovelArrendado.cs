namespace FinanceTracker.Data
{
    public class ImovelArrendado
    {
        public int Id { get; set; }
        public int AtivoId { get; set; }
        public string Designacao { get; set; }
        public string Localizacao { get; set; }
        public float ValorImovel { get; set; }
        public float ValorRenda { get; set; }
        public float ValorCondominio { get; set; }
        public float OutrasDespesas { get; set; }

        public AtivoFinanceiro AtivoFinanceiro { get; set; }
    }
}