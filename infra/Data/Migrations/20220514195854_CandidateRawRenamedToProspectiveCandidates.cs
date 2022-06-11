using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class CandidateRawRenamedToProspectiveCandidates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CandidatesRawData");

            migrationBuilder.CreateTable(
                name: "ProspectiveCandidates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryRef = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    Source = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    ResumeId = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    ResumeTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CandidateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Age = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PhoneNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    AlternatePhoneNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlternateEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Education = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ctc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkExperience = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProspectiveCandidates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProspectiveCandidates_ResumeId_Source",
                table: "ProspectiveCandidates",
                columns: new[] { "ResumeId", "Source" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProspectiveCandidates");

            migrationBuilder.CreateTable(
                name: "CandidatesRawData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Age = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    AlternateEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlternatePhoneNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    CandidateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Ctc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Education = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    ResumeId = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    ResumeTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Source = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    WorkExperience = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidatesRawData", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CandidatesRawData_ResumeId_Source",
                table: "CandidatesRawData",
                columns: new[] { "ResumeId", "Source" },
                unique: true);
        }
    }
}
