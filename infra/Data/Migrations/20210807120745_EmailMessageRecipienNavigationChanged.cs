using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class EmailMessageRecipienNavigationChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailMessages_AppUser_RecipientId",
                table: "EmailMessages");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailMessages_AppUser_RecipientId",
                table: "EmailMessages",
                column: "RecipientId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailMessages_AppUser_RecipientId",
                table: "EmailMessages");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailMessages_AppUser_RecipientId",
                table: "EmailMessages",
                column: "RecipientId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
