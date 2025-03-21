using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FinanceTracker.Data;

namespace FinanceTracker.Models
{
    public class Utilizador
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string SenhaHash { get; set; }

        [Required]
        public string TipoUtilizador { get; set; }

        public ICollection<AtivoFinanceiro> AtivosFinanceiros { get; set; }
    }
}