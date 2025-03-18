using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTracker.Data;

[Table("depositoprazo")]
public class DepositoPrazo
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("ativo_id")]
    public int ativo_id { get; set; }

    [Column("valor")]
    public double valor { get; set; }

    [Column("banco")]
    public string banco { get; set; }

    [Column("numero_conta")]
    public string numero_conta { get; set; }

    [Column("titulares")]
    public string titulares { get; set; }

    [Column("taxa_juro_anual")]
    public double taxa_juro_anual { get; set; }
}