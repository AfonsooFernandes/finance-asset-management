﻿using FinanceTracker.Models;
using System.Collections.Generic;

namespace FinanceTracker.Data
{
    public class FundoInvestimento
    {
        public int Id { get; set; }
        public int AtivoId { get; set; }
        public string Nome { get; set; }
        public double Montante { get; set; }
        public double TaxaJuro { get; set; }

        public AtivoFinanceiro AtivoFinanceiro { get; set; }

        public ICollection<JurosMensaisFundo> JurosMensais { get; set; }
        
    }
}