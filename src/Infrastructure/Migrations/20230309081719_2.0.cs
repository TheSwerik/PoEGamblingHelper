using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrencyResult",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ChaosEquivalent = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyResult", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Result",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GemTradeDataId = table.Column<long>(type: "bigint", nullable: false),
                    CurrencyValue = table.Column<decimal>(type: "numeric", nullable: false),
                    CurrencyResultId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Result", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Result_CurrencyResult_CurrencyResultId",
                        column: x => x.CurrencyResultId,
                        principalTable: "CurrencyResult",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Result_GemTradeData_GemTradeDataId",
                        column: x => x.GemTradeDataId,
                        principalTable: "GemTradeData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Result_CurrencyResultId",
                table: "Result",
                column: "CurrencyResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Result_GemTradeDataId",
                table: "Result",
                column: "GemTradeDataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Result");

            migrationBuilder.DropTable(
                name: "CurrencyResult");
        }
    }
}
