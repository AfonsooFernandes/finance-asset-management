using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTracker.Data;

[Table("fundoinvestimento")]
public class FundoInvestimento
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("ativo_id")]
    public int ativo_id { get; set; }

    [Column("nome")]
    public string nome { get; set; }

    [Column("montante")]
    public double montante { get; set; }

    [Column("taxa_juro")]
    public double taxa_juro { get; set; }
}