using Microsoft.EntityFrameworkCore;
using FinanceTracker.Models;

namespace FinanceTracker.Data
{
    public class FinanceTrackerContext : DbContext
    {
        public FinanceTrackerContext(DbContextOptions<FinanceTrackerContext> options)
            : base(options)
        {
        }

        public DbSet<Utilizador> Utilizadores { get; set; }
        public DbSet<AtivoFinanceiro> AtivosFinanceiros { get; set; }
        public DbSet<DepositoPrazo> DepositosPrazo { get; set; }
        public DbSet<FundoInvestimento> FundosInvestimento { get; set; }
        public DbSet<JurosMensaisFundo> JurosMensaisFundos { get; set; }
        public DbSet<ImovelArrendado> ImoveisArrendados { get; set; }
        public DbSet<PagamentoImpostos> PagamentosImpostos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            modelBuilder.Entity<Utilizador>()
                .ToTable("Utilizadores")
                .HasMany(u => u.AtivosFinanceiros)
                .WithOne(a => a.Utilizador)
                .HasForeignKey(a => a.UtilizadorId);

            modelBuilder.Entity<AtivoFinanceiro>()
                .ToTable("AtivosFinanceiros");

            modelBuilder.Entity<DepositoPrazo>()
                .ToTable("DepositosPrazo")
                .HasOne(dp => dp.AtivoFinanceiro)
                .WithOne(a => a.DepositoPrazo)
                .HasForeignKey<DepositoPrazo>(dp => dp.AtivoId);

            modelBuilder.Entity<FundoInvestimento>()
                .ToTable("FundosInvestimento")
                .HasOne(fi => fi.AtivoFinanceiro)
                .WithOne(a => a.FundoInvestimento)
                .HasForeignKey<FundoInvestimento>(fi => fi.AtivoId);

            modelBuilder.Entity<JurosMensaisFundo>()
                .ToTable("JurosMensaisFundos")
                .HasOne(jmf => jmf.FundoInvestimento)
                .WithMany(fi => fi.JurosMensais)
                .HasForeignKey(jmf => jmf.FundoId);

            modelBuilder.Entity<ImovelArrendado>()
                .ToTable("ImoveisArrendados")
                .HasOne(ia => ia.AtivoFinanceiro)
                .WithOne(a => a.ImovelArrendado)
                .HasForeignKey<ImovelArrendado>(ia => ia.AtivoId);

            modelBuilder.Entity<PagamentoImpostos>()
                .ToTable("PagamentosImpostos")
                .HasOne(pi => pi.AtivoFinanceiro)
                .WithMany()
                .HasForeignKey(pi => pi.AtivoId);
        }
    }
}