using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class EmailMessageAppUserRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
