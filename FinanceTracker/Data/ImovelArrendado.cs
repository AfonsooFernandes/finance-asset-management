using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTracker.Data;

[Table("imovelarrendado")]
public class ImovelArrendado
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("ativo_id")]
    public int ativo_id { get; set; }

    [Column("designacao")]
    public string designacao { get; set; }

    [Column("localizacao")]
    public string localizacao { get; set; }

    [Column("valor_imovel")]
    public double valor_imovel { get; set; }

    [Column("valor_renda")]
    public double valor_renda { get; set; }

    [Column("valor_condominio")]
    public double valor_condominio { get; set; }

    [Column("outras_despesas")]
    public double outras_despesas { get; set; }
}