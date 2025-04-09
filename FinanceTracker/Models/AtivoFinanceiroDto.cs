using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Models
{
    public class AtivoFinanceiroDto
    {
        public int Id { get; set; }
        public int UtilizadorId { get; set; }

        [Required(ErrorMessage = "O tipo do ativo financeiro é obrigatório.")]
        [StringLength(50, ErrorMessage = "O tipo do ativo não pode ter mais de 50 caracteres.")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "A data de início é obrigatória.")]
        public DateTime DataInicio { get; set; }

        [Required(ErrorMessage = "A duração é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "A duração deve ser maior que zero.")]
        public int Duracao { get; set; }

        [Required(ErrorMessage = "O imposto é obrigatório.")]
        [Range(0, float.MaxValue, ErrorMessage = "O imposto deve ser um valor positivo.")]
        public float Imposto { get; set; }
    }
}