using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class Checklist_AddedExceptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "Charges",
                table: "ChecklistHRs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ChargesAgreed",
                table: "ChecklistHRs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "ExceptionApproved",
                table: "ChecklistHRs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ExceptionApprovedBy",
                table: "ChecklistHRs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExceptionApprovedOn",
                table: "ChecklistHRs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Charges",
                table: "ChecklistHRs");

            migrationBuilder.DropColumn(
                name: "ChargesAgreed",
                table: "ChecklistHRs");

            migrationBuilder.DropColumn(
                name: "ExceptionApproved",
                table: "ChecklistHRs");

            migrationBuilder.DropColumn(
                name: "ExceptionApprovedBy",
                table: "ChecklistHRs");

            migrationBuilder.DropColumn(
                name: "ExceptionApprovedOn",
                table: "ChecklistHRs");

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
    }
}
