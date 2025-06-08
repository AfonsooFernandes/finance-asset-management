
using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Models
{
    public class PagamentoImpostosDto
    {
        public int Id { get; set; }
        public int AtivoId { get; set; }

        [Required(ErrorMessage = "A data do pagamento é obrigatória.")]
        public DateTime DataPagamento { get; set; }

        [Required(ErrorMessage = "O valor do pagamento é obrigatório.")]
        [Range(0, double.MaxValue, ErrorMessage = "O valor deve ser um valor positivo.")]
        public decimal Valor { get; set; }  
    }
}