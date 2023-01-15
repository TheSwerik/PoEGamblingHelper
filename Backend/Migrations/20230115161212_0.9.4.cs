using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class _094 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ChaosEquivalent = table.Column<decimal>(type: "numeric", nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GemData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GemData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leagues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Version = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leagues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TempleCost",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChaosValue = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempleCost", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GemTradeData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    GemLevel = table.Column<int>(type: "integer", nullable: false),
                    GemQuality = table.Column<int>(type: "integer", nullable: false),
                    Corrupted = table.Column<bool>(type: "boolean", nullable: false),
                    DetailsId = table.Column<string>(type: "text", nullable: false),
                    ChaosValue = table.Column<decimal>(type: "numeric", nullable: false),
                    ExaltedValue = table.Column<decimal>(type: "numeric", nullable: false),
                    DivineValue = table.Column<decimal>(type: "numeric", nullable: false),
                    ListingCount = table.Column<int>(type: "integer", nullable: false),
                    GemDataId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GemTradeData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GemTradeData_GemData_GemDataId",
                        column: x => x.GemDataId,
                        principalTable: "GemData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GemTradeData_GemDataId",
                table: "GemTradeData",
                column: "GemDataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "GemTradeData");

            migrationBuilder.DropTable(
                name: "Leagues");

            migrationBuilder.DropTable(
                name: "TempleCost");

            migrationBuilder.DropTable(
                name: "GemData");
        }
    }
}
