using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FinanceTracker.Migrations
{
    public partial class UpdateFinanceTrackerContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "Utilizadores",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    SenhaHash = table.Column<string>(type: "text", nullable: false),
                    TipoUtilizador = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizadores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AtivosFinanceiros",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UtilizadorId = table.Column<int>(type: "integer", nullable: false),
                    Tipo = table.Column<string>(type: "text", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Duracao = table.Column<int>(type: "integer", nullable: false),
                    Imposto = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtivosFinanceiros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtivosFinanceiros_Utilizadores_UtilizadorId",
                        column: x => x.UtilizadorId,
                        principalSchema: "public",
                        principalTable: "Utilizadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepositosPrazo",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AtivoId = table.Column<int>(type: "integer", nullable: false),
                    Valor = table.Column<float>(type: "FLOAT", nullable: false),
                    Banco = table.Column<string>(type: "text", nullable: false),
                    NumeroConta = table.Column<string>(type: "text", nullable: false),
                    Titulares = table.Column<string>(type: "text", nullable: false),
                    TaxaJuroAnual = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositosPrazo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepositosPrazo_AtivosFinanceiros_AtivoId",
                        column: x => x.AtivoId,
                        principalSchema: "public",
                        principalTable: "AtivosFinanceiros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FundosInvestimento",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AtivoId = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Montante = table.Column<float>(type: "FLOAT", nullable: false),
                    TaxaJuro = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundosInvestimento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FundosInvestimento_AtivosFinanceiros_AtivoId",
                        column: x => x.AtivoId,
                        principalSchema: "public",
                        principalTable: "AtivosFinanceiros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImoveisArrendados",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AtivoId = table.Column<int>(type: "integer", nullable: false),
                    Designacao = table.Column<string>(type: "text", nullable: false),
                    Localizacao = table.Column<string>(type: "text", nullable: false),
                    ValorImovel = table.Column<float>(type: "FLOAT", nullable: false),
                    ValorRenda = table.Column<float>(type: "FLOAT", nullable: false),
                    ValorCondominio = table.Column<float>(type: "real", nullable: false),
                    OutrasDespesas = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImoveisArrendados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImoveisArrendados_AtivosFinanceiros_AtivoId",
                        column: x => x.AtivoId,
                        principalSchema: "public",
                        principalTable: "AtivosFinanceiros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PagamentosImpostos",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AtivoId = table.Column<int>(type: "integer", nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Valor = table.Column<float>(type: "FLOAT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagamentosImpostos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PagamentosImpostos_AtivosFinanceiros_AtivoId",
                        column: x => x.AtivoId,
                        principalSchema: "public",
                        principalTable: "AtivosFinanceiros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JurosMensaisFundos",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FundoId = table.Column<int>(type: "integer", nullable: false),
                    Mes = table.Column<int>(type: "integer", nullable: false),
                    Ano = table.Column<int>(type: "integer", nullable: false),
                    Taxa = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JurosMensaisFundos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JurosMensaisFundos_FundosInvestimento_FundoId",
                        column: x => x.FundoId,
                        principalSchema: "public",
                        principalTable: "FundosInvestimento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AtivosFinanceiros_UtilizadorId",
                schema: "public",
                table: "AtivosFinanceiros",
                column: "UtilizadorId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositosPrazo_AtivoId",
                schema: "public",
                table: "DepositosPrazo",
                column: "AtivoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FundosInvestimento_AtivoId",
                schema: "public",
                table: "FundosInvestimento",
                column: "AtivoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImoveisArrendados_AtivoId",
                schema: "public",
                table: "ImoveisArrendados",
                column: "AtivoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JurosMensaisFundos_FundoId",
                schema: "public",
                table: "JurosMensaisFundos",
                column: "FundoId");

            migrationBuilder.CreateIndex(
                name: "IX_PagamentosImpostos_AtivoId",
                schema: "public",
                table: "PagamentosImpostos",
                column: "AtivoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepositosPrazo",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ImoveisArrendados",
                schema: "public");

            migrationBuilder.DropTable(
                name: "JurosMensaisFundos",
                schema: "public");

            migrationBuilder.DropTable(
                name: "PagamentosImpostos",
                schema: "public");

            migrationBuilder.DropTable(
                name: "FundosInvestimento",
                schema: "public");

            migrationBuilder.DropTable(
                name: "AtivosFinanceiros",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Utilizadores",
                schema: "public");
        }
    }
}
