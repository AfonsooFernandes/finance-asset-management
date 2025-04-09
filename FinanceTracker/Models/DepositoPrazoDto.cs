using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Models
{
    public class DepositoPrazoDto
    {
        public int Id { get; set; }
        public int AtivoId { get; set; }

        [Required(ErrorMessage = "O valor é obrigatório.")]
        [Range(0, float.MaxValue, ErrorMessage = "O valor deve ser um valor positivo.")]
        public float Valor { get; set; }

        [Required(ErrorMessage = "O banco é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do banco não pode ter mais de 100 caracteres.")]
        public string Banco { get; set; }

        [Required(ErrorMessage = "O número da conta é obrigatório.")]
        [StringLength(50, ErrorMessage = "O número da conta não pode ter mais de 50 caracteres.")]
        public string NumeroConta { get; set; }

        [Required(ErrorMessage = "Os titulares são obrigatórios.")]
        [StringLength(200, ErrorMessage = "O nome dos titulares não pode ter mais de 200 caracteres.")]
        public string Titulares { get; set; }

        [Required(ErrorMessage = "A taxa de juro anual é obrigatória.")]
        [Range(0, 100, ErrorMessage = "A taxa de juro deve ser entre 0 e 100.")]
        public float TaxaJuroAnual { get; set; }
    }
}