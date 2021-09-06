using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class MessageAppUserNavigationAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecipientId",
                table: "EmailMessages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderId",
                table: "EmailMessages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailMessages_RecipientId",
                table: "EmailMessages",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailMessages_SenderId",
                table: "EmailMessages",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailMessages_AppUser_RecipientId",
                table: "EmailMessages",
                column: "RecipientId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailMessages_AppUser_SenderId",
                table: "EmailMessages",
                column: "SenderId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailMessages_AppUser_RecipientId",
                table: "EmailMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailMessages_AppUser_SenderId",
                table: "EmailMessages");

            migrationBuilder.DropIndex(
                name: "IX_EmailMessages_RecipientId",
                table: "EmailMessages");

            migrationBuilder.DropIndex(
                name: "IX_EmailMessages_SenderId",
                table: "EmailMessages");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "EmailMessages");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "EmailMessages");
        }
    }
}
