using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class ChecklistHRItem_exceptionsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ExceptionApproved",
                table: "ChecklistHRItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ExceptionApprovedBy",
                table: "ChecklistHRItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExceptionApprovedOn",
                table: "ChecklistHRItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExceptionApproved",
                table: "ChecklistHRItems");

            migrationBuilder.DropColumn(
                name: "ExceptionApprovedBy",
                table: "ChecklistHRItems");

            migrationBuilder.DropColumn(
                name: "ExceptionApprovedOn",
                table: "ChecklistHRItems");
        }
    }
}
