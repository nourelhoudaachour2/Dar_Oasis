using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DarOasis.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFacture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateEcheance",
                table: "Factures",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "NumeroFacture",
                table: "Factures",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "PenaliteRetard",
                table: "Factures",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Promotion",
                table: "Factures",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Statut",
                table: "Factures",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateEcheance",
                table: "Factures");

            migrationBuilder.DropColumn(
                name: "NumeroFacture",
                table: "Factures");

            migrationBuilder.DropColumn(
                name: "PenaliteRetard",
                table: "Factures");

            migrationBuilder.DropColumn(
                name: "Promotion",
                table: "Factures");

            migrationBuilder.DropColumn(
                name: "Statut",
                table: "Factures");
        }
    }
}
