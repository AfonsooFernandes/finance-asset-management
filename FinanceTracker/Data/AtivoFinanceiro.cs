using System;
using System.Collections.Generic;
using FinanceTracker.Models;

namespace FinanceTracker.Data
{
    public class AtivoFinanceiro
    {
        public int Id { get; set; }
        public int UtilizadorId { get; set; }
        public string Tipo { get; set; }
        public DateTime DataInicio { get; set; }
        public int Duracao { get; set; }
        public float Imposto { get; set; }

        public Utilizador Utilizador { get; set; }

        public DepositoPrazo DepositoPrazo { get; set; }
        public FundoInvestimento FundoInvestimento { get; set; }
        public ImovelArrendado ImovelArrendado { get; set; }
    }
}