using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DarOasis.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCapacite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacite",
                table: "Chambres");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Capacite",
                table: "Chambres",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
