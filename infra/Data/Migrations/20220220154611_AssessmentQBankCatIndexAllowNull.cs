using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class AssessmentQBankCatIndexAllowNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AssessmentQBank_CategoryId",
                table: "AssessmentQBank");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentQBank_CategoryId",
                table: "AssessmentQBank",
                column: "CategoryId",
                unique: true,
                filter: "CategoryId Is Not NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AssessmentQBank_CategoryId",
                table: "AssessmentQBank");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentQBank_CategoryId",
                table: "AssessmentQBank",
                column: "CategoryId",
                unique: true);
        }
    }
}
