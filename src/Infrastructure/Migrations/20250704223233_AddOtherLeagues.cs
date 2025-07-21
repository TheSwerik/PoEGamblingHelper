using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoEGamblingHelper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOtherLeagues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Result_GemTradeData_GemTradeDataId",
                table: "Result");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TempleCost",
                table: "TempleCost");

            migrationBuilder.DropIndex(
                name: "IX_Result_GemTradeDataId",
                table: "Result");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GemTradeData",
                table: "GemTradeData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currency",
                table: "Currency");

            migrationBuilder.AddColumn<string>(
                name: "League",
                table: "TempleCost",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GemTradeDataLeague",
                table: "Result",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "League",
                table: "GemTradeData",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "League",
                table: "Currency",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TempleCost",
                table: "TempleCost",
                columns: new[] { "Id", "League" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_GemTradeData",
                table: "GemTradeData",
                columns: new[] { "Id", "League" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currency",
                table: "Currency",
                columns: new[] { "Id", "League" });

            migrationBuilder.CreateIndex(
                name: "IX_Result_GemTradeDataId_GemTradeDataLeague",
                table: "Result",
                columns: new[] { "GemTradeDataId", "GemTradeDataLeague" });

            migrationBuilder.AddForeignKey(
                name: "FK_Result_GemTradeData_GemTradeDataId_GemTradeDataLeague",
                table: "Result",
                columns: new[] { "GemTradeDataId", "GemTradeDataLeague" },
                principalTable: "GemTradeData",
                principalColumns: new[] { "Id", "League" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Result_GemTradeData_GemTradeDataId_GemTradeDataLeague",
                table: "Result");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TempleCost",
                table: "TempleCost");

            migrationBuilder.DropIndex(
                name: "IX_Result_GemTradeDataId_GemTradeDataLeague",
                table: "Result");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GemTradeData",
                table: "GemTradeData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currency",
                table: "Currency");

            migrationBuilder.DropColumn(
                name: "League",
                table: "TempleCost");

            migrationBuilder.DropColumn(
                name: "GemTradeDataLeague",
                table: "Result");

            migrationBuilder.DropColumn(
                name: "League",
                table: "GemTradeData");

            migrationBuilder.DropColumn(
                name: "League",
                table: "Currency");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TempleCost",
                table: "TempleCost",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GemTradeData",
                table: "GemTradeData",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currency",
                table: "Currency",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Result_GemTradeDataId",
                table: "Result",
                column: "GemTradeDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_Result_GemTradeData_GemTradeDataId",
                table: "Result",
                column: "GemTradeDataId",
                principalTable: "GemTradeData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
