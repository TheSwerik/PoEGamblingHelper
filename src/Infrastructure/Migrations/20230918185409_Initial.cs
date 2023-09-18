using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PoEGamblingHelper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Entity<string>",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ChaosEquivalent = table.Column<decimal>(type: "numeric", nullable: true),
                    Icon = table.Column<string>(type: "text", nullable: true),
                    CurrencyResult_Name = table.Column<string>(type: "text", nullable: true),
                    CurrencyResult_ChaosEquivalent = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entity<string>", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entity<Guid>",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: true),
                    Views = table.Column<long>(type: "bigint", nullable: true),
                    IpHash = table.Column<byte[]>(type: "bytea", nullable: true),
                    TimeStamp = table.Column<DateOnly>(type: "date", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Icon = table.Column<string>(type: "text", nullable: true),
                    League_Name = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Version = table.Column<string>(type: "text", nullable: true),
                    GemTradeDataId = table.Column<long>(type: "bigint", nullable: true),
                    CurrencyValue = table.Column<decimal>(type: "numeric", nullable: true),
                    CurrencyResultId = table.Column<string>(type: "text", nullable: true),
                    TempleCost_TimeStamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChaosValue = table.Column<decimal[]>(type: "numeric[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entity<Guid>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entity<Guid>_Entity<string>_CurrencyResultId",
                        column: x => x.CurrencyResultId,
                        principalTable: "Entity<string>",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Entity<long>",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    GemLevel = table.Column<int>(type: "integer", nullable: true),
                    GemQuality = table.Column<int>(type: "integer", nullable: true),
                    Corrupted = table.Column<bool>(type: "boolean", nullable: true),
                    DetailsId = table.Column<string>(type: "text", nullable: true),
                    ChaosValue = table.Column<decimal>(type: "numeric", nullable: true),
                    ExaltedValue = table.Column<decimal>(type: "numeric", nullable: true),
                    DivineValue = table.Column<decimal>(type: "numeric", nullable: true),
                    ListingCount = table.Column<int>(type: "integer", nullable: true),
                    GemDataId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entity<long>", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entity<long>_Entity<Guid>_GemDataId",
                        column: x => x.GemDataId,
                        principalTable: "Entity<Guid>",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entity<Guid>_CurrencyResultId",
                table: "Entity<Guid>",
                column: "CurrencyResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Entity<Guid>_GemTradeDataId",
                table: "Entity<Guid>",
                column: "GemTradeDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Entity<long>_GemDataId",
                table: "Entity<long>",
                column: "GemDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entity<Guid>_Entity<long>_GemTradeDataId",
                table: "Entity<Guid>",
                column: "GemTradeDataId",
                principalTable: "Entity<long>",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entity<Guid>_Entity<long>_GemTradeDataId",
                table: "Entity<Guid>");

            migrationBuilder.DropTable(
                name: "Entity<long>");

            migrationBuilder.DropTable(
                name: "Entity<Guid>");

            migrationBuilder.DropTable(
                name: "Entity<string>");
        }
    }
}
