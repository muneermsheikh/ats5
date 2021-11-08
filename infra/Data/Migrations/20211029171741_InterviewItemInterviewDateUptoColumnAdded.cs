using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class InterviewItemInterviewDateUptoColumnAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InterviewDate",
                table: "InterviewItems",
                newName: "InterviewDateUpto");

            migrationBuilder.AddColumn<DateTime>(
                name: "InterviewDateFrom",
                table: "InterviewItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InterviewDateFrom",
                table: "InterviewItems");

            migrationBuilder.RenameColumn(
                name: "InterviewDateUpto",
                table: "InterviewItems",
                newName: "InterviewDate");
        }
    }
}
