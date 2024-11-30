using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OopProject.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestStatusEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
    UPDATE Requests
    SET Status = 
        CASE 
            WHEN Status = 'Pending' THEN 0
            WHEN Status = 'Processing' THEN 1
            WHEN Status = 'BookedSuccessfully' THEN 2
            WHEN Status = 'Failed' THEN 3
        END;
");
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Requests",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
