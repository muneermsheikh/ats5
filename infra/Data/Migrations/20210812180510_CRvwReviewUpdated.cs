using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class CRvwReviewUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaskType",
                table: "TaskItems",
                newName: "TaskTypeId");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationNo",
                table: "Tasks",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CandidateId",
                table: "Tasks",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderNo",
                table: "Tasks",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ForwardedToHRDeptOn",
                table: "Orders",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationNo",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CandidateId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "OrderNo",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ForwardedToHRDeptOn",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "TaskTypeId",
                table: "TaskItems",
                newName: "TaskType");
        }
    }
}
