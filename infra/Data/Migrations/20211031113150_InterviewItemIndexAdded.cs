using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class InterviewItemIndexAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_InterviewItems_OrderItemId",
                table: "InterviewItems",
                column: "OrderItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InterviewItemCandidates_ApplicationNo",
                table: "InterviewItemCandidates",
                column: "ApplicationNo");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewItemCandidates_CandidateId_InterviewItemId",
                table: "InterviewItemCandidates",
                columns: new[] { "CandidateId", "InterviewItemId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InterviewItems_OrderItemId",
                table: "InterviewItems");

            migrationBuilder.DropIndex(
                name: "IX_InterviewItemCandidates_ApplicationNo",
                table: "InterviewItemCandidates");

            migrationBuilder.DropIndex(
                name: "IX_InterviewItemCandidates_CandidateId_InterviewItemId",
                table: "InterviewItemCandidates");
        }
    }
}
