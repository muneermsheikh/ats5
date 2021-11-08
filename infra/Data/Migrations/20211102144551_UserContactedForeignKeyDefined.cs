using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class UserContactedForeignKeyDefined : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersContacted");

            migrationBuilder.AddColumn<int>(
                name: "CandidateId",
                table: "Candidates",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserContacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CandidateId = table.Column<int>(type: "int", nullable: false),
                    OrderItemId = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfContact = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoggedInUserId = table.Column<int>(type: "int", nullable: false),
                    UserPhoneNoContacted = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    enumContactResult = table.Column<int>(type: "int", nullable: false),
                    GistOfDiscussions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NextReminderOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContacts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_CandidateId",
                table: "Candidates",
                column: "CandidateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidates_UserContacts_CandidateId",
                table: "Candidates",
                column: "CandidateId",
                principalTable: "UserContacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidates_UserContacts_CandidateId",
                table: "Candidates");

            migrationBuilder.DropTable(
                name: "UserContacts");

            migrationBuilder.DropIndex(
                name: "IX_Candidates_CandidateId",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "CandidateId",
                table: "Candidates");

            migrationBuilder.CreateTable(
                name: "UsersContacted",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CandidateId = table.Column<int>(type: "int", nullable: false),
                    DateOfContact = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GistOfDiscussions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoggedInUserId = table.Column<int>(type: "int", nullable: false),
                    NextReminderOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderItemId = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserPhoneNoContacted = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    enumContactResult = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersContacted", x => x.Id);
                });
        }
    }
}
