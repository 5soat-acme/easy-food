using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EF.Pagamentos.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoStatusPagamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Pagamentos",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Pagamentos");
        }
    }
}
