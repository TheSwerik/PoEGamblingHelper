using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_View",
                table: "View");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "View",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_View",
                table: "View",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_View",
                table: "View");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "View");

            migrationBuilder.AddPrimaryKey(
                name: "PK_View",
                table: "View",
                column: "IpHash");
        }
    }
}
