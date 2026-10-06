using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DarOasis.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmployePoste : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Employes",
                newName: "Poste");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Poste",
                table: "Employes",
                newName: "Role");
        }
    }
}
