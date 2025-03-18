using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTracker.Data;

[Table("ativofinanceiro")]
public class AtivoFinanceiro
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("utilizador_id")]
    public int utilizador_id { get; set; }

    [Column("tipo")]
    public string tipo { get; set; }

    [Column("data_inicio")]
    public DateTime data_inicio { get; set; }

    [Column("duracao")]
    public int duracao { get; set; }

    [Column("imposto")]
    public double imposto { get; set; }
}