using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class CVRefNavigationsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CVRefs_Candidates_CandidateId",
                table: "CVRefs");

            migrationBuilder.DropForeignKey(
                name: "FK_CVRefs_OrderItems_OrderItemId1",
                table: "CVRefs");

            migrationBuilder.DropIndex(
                name: "IX_CVRefs_OrderItemId1",
                table: "CVRefs");

            migrationBuilder.DropColumn(
                name: "OrderItemId1",
                table: "CVRefs");

            migrationBuilder.DropColumn(
                name: "OrderItemId2",
                table: "CVRefs");

            migrationBuilder.AddColumn<int>(
                name: "OrderItemId",
                table: "OrderItems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CVRefId",
                table: "Candidates",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderItemId",
                table: "OrderItems",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_CVRefId",
                table: "Candidates",
                column: "CVRefId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidates_CVRefs_CVRefId",
                table: "Candidates",
                column: "CVRefId",
                principalTable: "CVRefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_CVRefs_OrderItemId",
                table: "OrderItems",
                column: "OrderItemId",
                principalTable: "CVRefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidates_CVRefs_CVRefId",
                table: "Candidates");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_CVRefs_OrderItemId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_OrderItemId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_Candidates_CVRefId",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "OrderItemId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CVRefId",
                table: "Candidates");

            migrationBuilder.AddColumn<int>(
                name: "OrderItemId1",
                table: "CVRefs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderItemId2",
                table: "CVRefs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CVRefs_OrderItemId1",
                table: "CVRefs",
                column: "OrderItemId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CVRefs_Candidates_CandidateId",
                table: "CVRefs",
                column: "CandidateId",
                principalTable: "Candidates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CVRefs_OrderItems_OrderItemId1",
                table: "CVRefs",
                column: "OrderItemId1",
                principalTable: "OrderItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
