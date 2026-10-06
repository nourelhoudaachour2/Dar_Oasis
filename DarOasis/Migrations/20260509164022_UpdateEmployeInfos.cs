using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DarOasis.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmployeInfos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Adresse",
                table: "Employes",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateEmbauche",
                table: "Employes",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateNaissance",
                table: "Employes",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Experience",
                table: "Employes",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "Salaire",
                table: "Employes",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adresse",
                table: "Employes");

            migrationBuilder.DropColumn(
                name: "DateEmbauche",
                table: "Employes");

            migrationBuilder.DropColumn(
                name: "DateNaissance",
                table: "Employes");

            migrationBuilder.DropColumn(
                name: "Experience",
                table: "Employes");

            migrationBuilder.DropColumn(
                name: "Salaire",
                table: "Employes");
        }
    }
}
