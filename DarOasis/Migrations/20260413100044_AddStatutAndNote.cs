using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DarOasis.Migrations
{
    /// <inheritdoc />
    public partial class AddStatutAndNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Note",
                table: "Chambres",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Statut",
                table: "Chambres",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Chambres");

            migrationBuilder.DropColumn(
                name: "Statut",
                table: "Chambres");
        }
    }
}
