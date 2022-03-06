using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class DLFwd_CategoryAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateForwarded",
                table: "DLForwards",
                newName: "DateTimeForwarded");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "DLForwards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOnlyForwarded",
                table: "DLForwards",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_DLForwards_OrderItemId_CustomerOfficialId_DateOnlyForwarded",
                table: "DLForwards",
                columns: new[] { "OrderItemId", "CustomerOfficialId", "DateOnlyForwarded" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DLForwards_OrderItemId_CustomerOfficialId_DateOnlyForwarded",
                table: "DLForwards");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "DLForwards");

            migrationBuilder.DropColumn(
                name: "DateOnlyForwarded",
                table: "DLForwards");

            migrationBuilder.RenameColumn(
                name: "DateTimeForwarded",
                table: "DLForwards",
                newName: "DateForwarded");
        }
    }
}
