using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class ContactResultIndexChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ContactResults_Name",
                table: "ContactResults");

            migrationBuilder.AlterColumn<string>(
                name: "PersonType",
                table: "ContactResults",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactResults_Name_PersonType",
                table: "ContactResults",
                columns: new[] { "Name", "PersonType" },
                unique: true,
                filter: "[PersonType] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ContactResults_Name_PersonType",
                table: "ContactResults");

            migrationBuilder.AlterColumn<string>(
                name: "PersonType",
                table: "ContactResults",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactResults_Name",
                table: "ContactResults",
                column: "Name",
                unique: true);
        }
    }
}
