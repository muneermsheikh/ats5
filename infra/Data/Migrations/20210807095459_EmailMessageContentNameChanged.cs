using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class EmailMessageContentNameChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailMessages_AppUser_RecipientId",
                table: "EmailMessages");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropColumn(
                name: "MessgeGroup",
                table: "EmailMessages");

            migrationBuilder.DropColumn(
                name: "RecipientName",
                table: "EmailMessages");

            migrationBuilder.RenameColumn(
                name: "SenderDisplayName",
                table: "EmailMessages",
                newName: "MessageGroup");

            migrationBuilder.RenameColumn(
                name: "MessageContent",
                table: "EmailMessages",
                newName: "Content");

            migrationBuilder.AlterColumn<string>(
                name: "RecipientId",
                table: "EmailMessages",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecipientUserName",
                table: "EmailMessages",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SenderUserName",
                table: "EmailMessages",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailMessages_AppUser_RecipientId",
                table: "EmailMessages",
                column: "RecipientId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailMessages_AppUser_RecipientId",
                table: "EmailMessages");

            migrationBuilder.DropColumn(
                name: "RecipientUserName",
                table: "EmailMessages");

            migrationBuilder.DropColumn(
                name: "SenderUserName",
                table: "EmailMessages");

            migrationBuilder.RenameColumn(
                name: "MessageGroup",
                table: "EmailMessages",
                newName: "SenderDisplayName");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "EmailMessages",
                newName: "MessageContent");

            migrationBuilder.AlterColumn<string>(
                name: "RecipientId",
                table: "EmailMessages",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "MessgeGroup",
                table: "EmailMessages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecipientName",
                table: "EmailMessages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    DateRead = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MessageSent = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RecipientDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    RecipientId = table.Column<string>(type: "TEXT", nullable: false),
                    RecipientUsername = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SenderDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    SenderId = table.Column<string>(type: "TEXT", nullable: true),
                    SenderUsername = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_AppUser_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_AppUser_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RecipientId",
                table: "Messages",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailMessages_AppUser_RecipientId",
                table: "EmailMessages",
                column: "RecipientId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
