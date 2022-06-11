using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class contactResultREsultNameAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactResultName",
                table: "UserHistoryItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoggedInUserName",
                table: "UserHistoryItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactResultName",
                table: "UserHistoryItems");

            migrationBuilder.DropColumn(
                name: "LoggedInUserName",
                table: "UserHistoryItems");
        }
    }
}
