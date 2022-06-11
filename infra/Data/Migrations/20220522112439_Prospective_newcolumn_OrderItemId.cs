using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class Prospective_newcolumn_OrderItemId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Natioality",
                table: "ProspectiveCandidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderItemId",
                table: "ProspectiveCandidates",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Natioality",
                table: "ProspectiveCandidates");

            migrationBuilder.DropColumn(
                name: "OrderItemId",
                table: "ProspectiveCandidates");
        }
    }
}
