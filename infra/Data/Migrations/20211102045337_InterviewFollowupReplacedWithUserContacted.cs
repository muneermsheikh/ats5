using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class InterviewFollowupReplacedWithUserContacted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserContactedItems");

            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "UsersContacted");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "UsersContacted",
                newName: "enumContactResult");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfContact",
                table: "UsersContacted",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "GistOfDiscussions",
                table: "UsersContacted",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LoggedInUserId",
                table: "UsersContacted",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextReminderOn",
                table: "UsersContacted",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "UsersContacted",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserPhoneNoContacted",
                table: "UsersContacted",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfContact",
                table: "UsersContacted");

            migrationBuilder.DropColumn(
                name: "GistOfDiscussions",
                table: "UsersContacted");

            migrationBuilder.DropColumn(
                name: "LoggedInUserId",
                table: "UsersContacted");

            migrationBuilder.DropColumn(
                name: "NextReminderOn",
                table: "UsersContacted");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "UsersContacted");

            migrationBuilder.DropColumn(
                name: "UserPhoneNoContacted",
                table: "UsersContacted");

            migrationBuilder.RenameColumn(
                name: "enumContactResult",
                table: "UsersContacted",
                newName: "AppUserId");

            migrationBuilder.AddColumn<bool>(
                name: "IsValid",
                table: "UsersContacted",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "UserContactedItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateOfContact = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoggedInUserId = table.Column<int>(type: "int", nullable: false),
                    NextReminderOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserContactedId = table.Column<int>(type: "int", nullable: false),
                    enumContactResult = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContactedItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserContactedItems_UsersContacted_UserContactedId",
                        column: x => x.UserContactedId,
                        principalTable: "UsersContacted",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserContactedItems_UserContactedId",
                table: "UserContactedItems",
                column: "UserContactedId");
        }
    }
}
