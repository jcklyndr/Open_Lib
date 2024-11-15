using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OopProject.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAdminRequestRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Admins_AdminId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_AdminId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Requests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "Requests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_AdminId",
                table: "Requests",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Admins_AdminId",
                table: "Requests",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "AdminId");
        }
    }
}
