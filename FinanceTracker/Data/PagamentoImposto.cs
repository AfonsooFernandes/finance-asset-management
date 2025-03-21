using System;

namespace FinanceTracker.Data
{
    public class PagamentoImpostos
    {
        public int Id { get; set; }
        public int AtivoId { get; set; }
        public DateTime DataPagamento { get; set; }
        public float Valor { get; set; }

        public AtivoFinanceiro AtivoFinanceiro { get; set; }
    }
}