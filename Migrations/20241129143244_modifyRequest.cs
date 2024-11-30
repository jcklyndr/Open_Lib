using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OopProject.Migrations
{
    /// <inheritdoc />
    public partial class modifyRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Requests");
        }
    }
}
