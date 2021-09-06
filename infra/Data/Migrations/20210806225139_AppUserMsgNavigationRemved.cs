using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class AppUserMsgNavigationRemved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AppUser_RecipientId",
                table: "Messages");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AppUser_RecipientId",
                table: "Messages",
                column: "RecipientId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AppUser_RecipientId",
                table: "Messages");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AppUser_RecipientId",
                table: "Messages",
                column: "RecipientId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
