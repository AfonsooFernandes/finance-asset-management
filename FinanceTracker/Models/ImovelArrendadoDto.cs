using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Models
{
    public class ImovelArrendadoDto
    {
        public int Id { get; set; }
        public int AtivoId { get; set; }

        [Required(ErrorMessage = "A designação do imóvel é obrigatória.")]
        [StringLength(200, ErrorMessage = "A designação não pode ter mais de 200 caracteres.")]
        public string Designacao { get; set; }

        [Required(ErrorMessage = "A localização do imóvel é obrigatória.")]
        [StringLength(300, ErrorMessage = "A localização não pode ter mais de 300 caracteres.")]
        public string Localizacao { get; set; }

        [Required(ErrorMessage = "O valor do imóvel é obrigatório.")]
        [Range(0, float.MaxValue, ErrorMessage = "O valor do imóvel deve ser um valor positivo.")]
        public double ValorImovel { get; set; }

        [Required(ErrorMessage = "O valor da renda é obrigatório.")]
        [Range(0, float.MaxValue, ErrorMessage = "O valor da renda deve ser um valor positivo.")]
        public double ValorRenda { get; set; }

        [Required(ErrorMessage = "O valor do condomínio é obrigatório.")]
        [Range(0, float.MaxValue, ErrorMessage = "O valor do condomínio deve ser um valor positivo.")]
        public double ValorCondominio { get; set; }

        [Required(ErrorMessage = "Outras despesas são obrigatórias.")]
        [Range(0, float.MaxValue, ErrorMessage = "As outras despesas devem ser um valor positivo.")]
        public double OutrasDespesas { get; set; }
    }
}