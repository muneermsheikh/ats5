using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class DeployStatusChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "Deploys",
                newName: "StageId");

            migrationBuilder.RenameColumn(
                name: "NextStatusId",
                table: "Deploys",
                newName: "NextStageId");

            migrationBuilder.RenameColumn(
                name: "NextEstimatedStatusDate",
                table: "Deploys",
                newName: "NextEstimatedStageDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StageId",
                table: "Deploys",
                newName: "StatusId");

            migrationBuilder.RenameColumn(
                name: "NextStageId",
                table: "Deploys",
                newName: "NextStatusId");

            migrationBuilder.RenameColumn(
                name: "NextEstimatedStageDate",
                table: "Deploys",
                newName: "NextEstimatedStatusDate");
        }
    }
}
