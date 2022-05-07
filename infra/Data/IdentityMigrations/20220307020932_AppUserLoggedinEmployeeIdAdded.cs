using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.IdentityMigrations
{
    public partial class AppUserLoggedinEmployeeIdAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "loggedInEmployeeId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "loggedInEmployeeId",
                table: "AspNetUsers");
        }
    }
}
