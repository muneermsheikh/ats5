using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class UserHistoryIndexRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserHistories_PersonId_PersonType",
                table: "UserHistories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserHistories_PersonId_PersonType",
                table: "UserHistories",
                columns: new[] { "PersonId", "PersonType" },
                unique: true,
                filter: "PersonId > 0");
        }
    }
}
