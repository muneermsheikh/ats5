using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class MessagesAndCompose : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "SelectionDecisions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "AppUserIdNotEnforced",
                table: "Candidates",
                type: "INTEGER",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Candidates",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NotificationDesired",
                table: "Candidates",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "AppUserRole",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AppUserRole",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AppRole",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MessgeGroup = table.Column<string>(type: "TEXT", nullable: true),
                    SenderEmailAddress = table.Column<string>(type: "TEXT", nullable: true),
                    SenderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    RecipientName = table.Column<string>(type: "TEXT", nullable: true),
                    RecipientEmailAddress = table.Column<string>(type: "TEXT", nullable: false),
                    CcEmailAddress = table.Column<string>(type: "TEXT", nullable: true),
                    BccEmailAddress = table.Column<string>(type: "TEXT", nullable: true),
                    Subject = table.Column<string>(type: "TEXT", nullable: false),
                    MessageContent = table.Column<string>(type: "TEXT", nullable: false),
                    MessageType = table.Column<string>(type: "TEXT", nullable: true),
                    DateReadOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MessageSentOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SenderDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    RecipientDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessageComposeSources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MessageType = table.Column<string>(type: "TEXT", nullable: true),
                    Mode = table.Column<string>(type: "TEXT", nullable: true),
                    SrNo = table.Column<int>(type: "INTEGER", nullable: false),
                    LineText = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageComposeSources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhoneMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AppUserId = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNo = table.Column<string>(type: "TEXT", maxLength: 14, nullable: false),
                    DateOfAdvise = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TextAdvised = table.Column<string>(type: "TEXT", maxLength: 160, nullable: true),
                    CompanyAdvised = table.Column<string>(type: "TEXT", nullable: true),
                    OfficialAdvised = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SMSMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    SMSDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PhoneNo = table.Column<string>(type: "TEXT", nullable: true),
                    SMSText = table.Column<string>(type: "TEXT", nullable: true),
                    DeliveryResult = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SMSMessages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserRole_RoleId",
                table: "AppUserRole",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserRole_AppRole_RoleId",
                table: "AppUserRole",
                column: "RoleId",
                principalTable: "AppRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserRole_AppRole_RoleId",
                table: "AppUserRole");

            migrationBuilder.DropTable(
                name: "AppRole");

            migrationBuilder.DropTable(
                name: "EmailMessages");

            migrationBuilder.DropTable(
                name: "MessageComposeSources");

            migrationBuilder.DropTable(
                name: "PhoneMessages");

            migrationBuilder.DropTable(
                name: "SMSMessages");

            migrationBuilder.DropIndex(
                name: "IX_AppUserRole_RoleId",
                table: "AppUserRole");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "SelectionDecisions");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "NotificationDesired",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "AppUserRole");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AppUserRole");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserIdNotEnforced",
                table: "Candidates",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "INTEGER");
        }
    }
}
