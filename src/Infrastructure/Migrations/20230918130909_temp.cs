using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class temp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "League");

            migrationBuilder.DropTable(
                name: "Result");

            migrationBuilder.DropTable(
                name: "TempleCost");

            migrationBuilder.DropTable(
                name: "View");

            migrationBuilder.DropTable(
                name: "CurrencyResult");

            migrationBuilder.DropTable(
                name: "GemTradeData");

            migrationBuilder.DropTable(
                name: "GemData");

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
                    IpHash = table.Column<byte[]>(type: "bytea", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Icon = table.Column<string>(type: "text", nullable: true),
                    League_Name = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Version = table.Column<string>(type: "text", nullable: true),
                    GemTradeDataId = table.Column<long>(type: "bigint", nullable: true),
                    CurrencyValue = table.Column<decimal>(type: "numeric", nullable: true),
                    CurrencyResultId = table.Column<string>(type: "text", nullable: true),
                    TempleCost_TimeStamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChaosValue = table.Column<string>(type: "text", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ChaosEquivalent = table.Column<decimal>(type: "numeric", nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyResult",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ChaosEquivalent = table.Column<decimal>(type: "numeric", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyResult", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GemData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GemData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "League",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Version = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_League", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TempleCost",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChaosValue = table.Column<string>(type: "text", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempleCost", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "View",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IpHash = table.Column<byte[]>(type: "bytea", nullable: false),
                    TimeStamp = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_View", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GemTradeData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChaosValue = table.Column<decimal>(type: "numeric", nullable: false),
                    Corrupted = table.Column<bool>(type: "boolean", nullable: false),
                    DetailsId = table.Column<string>(type: "text", nullable: false),
                    DivineValue = table.Column<decimal>(type: "numeric", nullable: false),
                    ExaltedValue = table.Column<decimal>(type: "numeric", nullable: false),
                    GemDataId = table.Column<Guid>(type: "uuid", nullable: true),
                    GemLevel = table.Column<int>(type: "integer", nullable: false),
                    GemQuality = table.Column<int>(type: "integer", nullable: false),
                    ListingCount = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Result",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrencyResultId = table.Column<string>(type: "text", nullable: true),
                    GemTradeDataId = table.Column<long>(type: "bigint", nullable: false),
                    CurrencyValue = table.Column<decimal>(type: "numeric", nullable: false)
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
                name: "IX_GemTradeData_GemDataId",
                table: "GemTradeData",
                column: "GemDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Result_CurrencyResultId",
                table: "Result",
                column: "CurrencyResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Result_GemTradeDataId",
                table: "Result",
                column: "GemTradeDataId");
        }
    }
}
