using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EF.Pagamentos.Infra.Migrations
{
    /// <inheritdoc />
    public partial class BaseInicialPagamentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FormasPagamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TipoFormaPagamento = table.Column<int>(type: "integer", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormasPagamento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pagamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PedidoId = table.Column<Guid>(type: "uuid", nullable: false),
                    FormaPagamentoId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataLancamento = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Valor = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagamentos_FormasPagamento_FormaPagamentoId",
                        column: x => x.FormaPagamentoId,
                        principalTable: "FormasPagamento",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "FormasPagamento",
                columns: new[] { "Id", "Descricao", "TipoFormaPagamento" },
                values: new object[,]
                {
                    { new Guid("17b6f2bc-520d-48e7-9008-2723fe5d2bcb"), "Pix", 1 },
                    { new Guid("29daa6de-862c-495f-93bf-153fd7d58dc0"), "Mercado Pago", 2 },
                    { new Guid("6a68e2ed-8e67-4f10-8ab5-fa7c67bf6371"), "Cartão de Crédito", 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormasPagamento_TipoFormaPagamento",
                table: "FormasPagamento",
                column: "TipoFormaPagamento",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_FormaPagamentoId",
                table: "Pagamentos",
                column: "FormaPagamentoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pagamentos");

            migrationBuilder.DropTable(
                name: "FormasPagamento");
        }
    }
}
