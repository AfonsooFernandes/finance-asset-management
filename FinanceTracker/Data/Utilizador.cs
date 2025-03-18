using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTracker.Data;

[Table("utilizador")]
public class Utilizador
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nome")]
    public string nome { get; set; }

    [Column("email")]
    public string email { get; set; }

    [Column("senha_hash")]
    public string senha_hash { get; set; }

    [Column("tipo_utilizador")]
    public string tipo_utilizador { get; set; }
}