using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class Task_TaskOwnerNameAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "TaskItems");

            migrationBuilder.AddColumn<string>(
                name: "AssignedToName",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaskOwnerName",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaskTypeName",
                table: "TaskItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "TaskItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedToName",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskOwnerName",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskTypeName",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "TaskItems");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "TaskItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
