using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class DeployStageIdChangedToEnumValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CVRefs_DeployStage_DeployStageId",
                table: "CVRefs");

            migrationBuilder.DropIndex(
                name: "IX_CVRefs_DeployStageId",
                table: "CVRefs");

            migrationBuilder.AddColumn<int>(
                name: "DeployStageId1",
                table: "CVRefs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CVRefs_DeployStageId1",
                table: "CVRefs",
                column: "DeployStageId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CVRefs_DeployStage_DeployStageId1",
                table: "CVRefs",
                column: "DeployStageId1",
                principalTable: "DeployStage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CVRefs_DeployStage_DeployStageId1",
                table: "CVRefs");

            migrationBuilder.DropIndex(
                name: "IX_CVRefs_DeployStageId1",
                table: "CVRefs");

            migrationBuilder.DropColumn(
                name: "DeployStageId1",
                table: "CVRefs");

            migrationBuilder.CreateIndex(
                name: "IX_CVRefs_DeployStageId",
                table: "CVRefs",
                column: "DeployStageId");

            migrationBuilder.AddForeignKey(
                name: "FK_CVRefs_DeployStage_DeployStageId",
                table: "CVRefs",
                column: "DeployStageId",
                principalTable: "DeployStage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
