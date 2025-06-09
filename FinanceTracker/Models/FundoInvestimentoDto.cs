using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Models
{
    public class FundoInvestimentoDto
    {
        public int Id { get; set; }
        public int AtivoId { get; set; }

        [Required(ErrorMessage = "O nome do fundo de investimento é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do fundo não pode ter mais de 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O montante é obrigatório.")]
        [Range(0, float.MaxValue, ErrorMessage = "O montante deve ser um valor positivo.")]
        public double Montante { get; set; }

        [Required(ErrorMessage = "A taxa de juro é obrigatória.")]
        [Range(0, 100, ErrorMessage = "A taxa de juro deve ser entre 0 e 100.")]
        public double TaxaJuro { get; set; }
    }
}