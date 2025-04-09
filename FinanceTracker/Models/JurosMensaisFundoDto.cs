using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Models
{
    public class JurosMensaisFundoDto
    {
        public int Id { get; set; }
        public int FundoId { get; set; }

        [Required(ErrorMessage = "O mês é obrigatório.")]
        [Range(1, 12, ErrorMessage = "O mês deve ser um valor entre 1 e 12.")]
        public int Mes { get; set; }

        [Required(ErrorMessage = "O ano é obrigatório.")]
        [Range(1900, 2100, ErrorMessage = "O ano deve ser um valor entre 1900 e 2100.")]
        public int Ano { get; set; }

        [Required(ErrorMessage = "A taxa de juros é obrigatória.")]
        [Range(0, float.MaxValue, ErrorMessage = "A taxa de juros deve ser um valor positivo.")]
        public float Taxa { get; set; }
    }
}