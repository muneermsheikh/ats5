using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class SelDecisionIndex2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SelectionDecisions_CVRefs_CVRefId",
                table: "SelectionDecisions");

            migrationBuilder.AddForeignKey(
                name: "FK_SelectionDecisions_CVRefs_CVRefId",
                table: "SelectionDecisions",
                column: "CVRefId",
                principalTable: "CVRefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SelectionDecisions_CVRefs_CVRefId",
                table: "SelectionDecisions");

            migrationBuilder.AddForeignKey(
                name: "FK_SelectionDecisions_CVRefs_CVRefId",
                table: "SelectionDecisions",
                column: "CVRefId",
                principalTable: "CVRefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
