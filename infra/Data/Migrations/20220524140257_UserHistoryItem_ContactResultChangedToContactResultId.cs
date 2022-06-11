using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class UserHistoryItem_ContactResultChangedToContactResultId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContactResult",
                table: "UserHistoryItems",
                newName: "ContactResultId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContactResultId",
                table: "UserHistoryItems",
                newName: "ContactResult");
        }
    }
}
