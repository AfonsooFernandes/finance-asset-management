using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Data;

public class FinanceTrackerContext : DbContext
{
    public FinanceTrackerContext(DbContextOptions<FinanceTrackerContext> options) : base(options)
    {
    }

    public DbSet<AtivoFinanceiro> AtivosFinanceiros { get; set; }
    public DbSet<DepositoPrazo> DepositosPrazo { get; set; }
    public DbSet<FundoInvestimento> FundosInvestimento { get; set; }
    public DbSet<ImovelArrendado> ImoveisArrendados { get; set; }
    public DbSet<JurosMensaisFundo> JurosMensaisFundos { get; set; }
    public DbSet<PagamentoImpostos> PagamentosImpostos { get; set; }
    public DbSet<Utilizador> Utilizadores { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AtivoFinanceiro>().ToTable("ativofinanceiro");
        modelBuilder.Entity<DepositoPrazo>().ToTable("depositoprazo");
        modelBuilder.Entity<FundoInvestimento>().ToTable("fundoinvestimento");
        modelBuilder.Entity<ImovelArrendado>().ToTable("imovelarrendado");
        modelBuilder.Entity<JurosMensaisFundo>().ToTable("jurosmensaisfundo");
        modelBuilder.Entity<PagamentoImpostos>().ToTable("pagamentoimpostos");
        modelBuilder.Entity<Utilizador>().ToTable("utilizador");

        base.OnModelCreating(modelBuilder);
    }
}