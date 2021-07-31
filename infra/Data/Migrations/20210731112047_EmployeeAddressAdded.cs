using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class EmployeeAddressAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "Employees",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Employees",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Employees",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address2",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Employees");
        }
    }
}
