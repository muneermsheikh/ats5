using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class UserHistoryPhoneNo_UniqueForNonBlanks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserHistories_PhoneNo",
                table: "UserHistories");

            migrationBuilder.CreateIndex(
                name: "IX_UserHistories_PhoneNo",
                table: "UserHistories",
                column: "PhoneNo",
                unique: true,
                filter: "PhoneNo != ''");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserHistories_PhoneNo",
                table: "UserHistories");

            migrationBuilder.CreateIndex(
                name: "IX_UserHistories_PhoneNo",
                table: "UserHistories",
                column: "PhoneNo",
                unique: true);
        }
    }
}
