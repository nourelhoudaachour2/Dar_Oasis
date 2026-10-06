using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DarOasis.Migrations
{
    /// <inheritdoc />
    public partial class LinkUtilisateurEmploye : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeId",
                table: "Utilisateurs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_EmployeId",
                table: "Utilisateurs",
                column: "EmployeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Utilisateurs_Employes_EmployeId",
                table: "Utilisateurs",
                column: "EmployeId",
                principalTable: "Employes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utilisateurs_Employes_EmployeId",
                table: "Utilisateurs");

            migrationBuilder.DropIndex(
                name: "IX_Utilisateurs_EmployeId",
                table: "Utilisateurs");

            migrationBuilder.DropColumn(
                name: "EmployeId",
                table: "Utilisateurs");
        }
    }
}
