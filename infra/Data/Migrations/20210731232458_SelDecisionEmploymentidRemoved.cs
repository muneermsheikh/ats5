using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class SelDecisionEmploymentidRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employments_SelectionDecisions_SelectionDecisionId",
                table: "Employments");

            migrationBuilder.DropForeignKey(
                name: "FK_SelectionDecisions_Employments_EmploymentId",
                table: "SelectionDecisions");

            migrationBuilder.DropIndex(
                name: "IX_SelectionDecisions_EmploymentId",
                table: "SelectionDecisions");

            migrationBuilder.DropColumn(
                name: "EmploymentId",
                table: "SelectionDecisions");

            migrationBuilder.AddForeignKey(
                name: "FK_Employments_SelectionDecisions_SelectionDecisionId",
                table: "Employments",
                column: "SelectionDecisionId",
                principalTable: "SelectionDecisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employments_SelectionDecisions_SelectionDecisionId",
                table: "Employments");

            migrationBuilder.AddColumn<int>(
                name: "EmploymentId",
                table: "SelectionDecisions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SelectionDecisions_EmploymentId",
                table: "SelectionDecisions",
                column: "EmploymentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employments_SelectionDecisions_SelectionDecisionId",
                table: "Employments",
                column: "SelectionDecisionId",
                principalTable: "SelectionDecisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SelectionDecisions_Employments_EmploymentId",
                table: "SelectionDecisions",
                column: "EmploymentId",
                principalTable: "Employments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
