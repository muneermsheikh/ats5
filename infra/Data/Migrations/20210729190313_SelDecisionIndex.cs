using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class SelDecisionIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SelectionDecisions_CVRefId",
                table: "SelectionDecisions",
                column: "CVRefId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SelectionDecisions_CVRefs_CVRefId",
                table: "SelectionDecisions",
                column: "CVRefId",
                principalTable: "CVRefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SelectionDecisions_CVRefs_CVRefId",
                table: "SelectionDecisions");

            migrationBuilder.DropIndex(
                name: "IX_SelectionDecisions_CVRefId",
                table: "SelectionDecisions");
        }
    }
}
