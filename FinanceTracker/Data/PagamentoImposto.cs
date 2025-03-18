using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTracker.Data;

[Table("pagamentoimpostos")]
public class PagamentoImpostos
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("ativo_id")]
    public int ativo_id { get; set; }

    [Column("data_pagamento")]
    public DateTime data_pagamento { get; set; }

    [Column("valor")]
    public double valor { get; set; }
}