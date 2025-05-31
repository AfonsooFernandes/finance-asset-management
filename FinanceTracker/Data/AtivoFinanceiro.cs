using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema; 
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
        
        [NotMapped]
        public decimal LucroTotalAntesImpostos => CalcularLucroAntesImpostos();

        [NotMapped]
        public decimal LucroTotalAposImpostos => LucroTotalAntesImpostos - (LucroTotalAntesImpostos * ((decimal)Imposto / 100));

        [NotMapped]
        public decimal LucroMensalMedioAntesImpostos => Duracao > 0 ? LucroTotalAntesImpostos / Duracao : 0m;

        [NotMapped]
        public decimal LucroMensalMedioAposImpostos => Duracao > 0 ? LucroTotalAposImpostos / Duracao : 0m;

        public Utilizador Utilizador { get; set; }

        public DepositoPrazo DepositoPrazo { get; set; }
        public FundoInvestimento FundoInvestimento { get; set; }
        public ImovelArrendado ImovelArrendado { get; set; }

        private decimal CalcularLucroAntesImpostos()
        {
            if (DepositoPrazo != null) return (decimal)DepositoPrazo.Valor * 0.05m * Duracao;
            if (FundoInvestimento != null) return (decimal)FundoInvestimento.Montante * 0.06m * Duracao;
            if (ImovelArrendado != null) return (decimal)ImovelArrendado.ValorRenda * Duracao;
            return 0m;
        }
    }
}