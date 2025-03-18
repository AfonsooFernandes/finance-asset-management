using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTracker.Data;

[Table("jurosmensaisfundo")]
public class JurosMensaisFundo
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("fundo_id")]
    public int fundo_id { get; set; }

    [Column("mes")]
    public int mes { get; set; }

    [Column("ano")]
    public int ano { get; set; }

    [Column("taxa")]
    public double taxa { get; set; }
}