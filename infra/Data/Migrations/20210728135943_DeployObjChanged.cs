using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class DeployObjChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CVRefs_DeployStages_DeployStageId",
                table: "CVRefs");

            migrationBuilder.DropForeignKey(
                name: "FK_Deploys_CVRefs_CVRefId1",
                table: "Deploys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeployStages",
                table: "DeployStages");

            migrationBuilder.DropColumn(
                name: "CVRefId2",
                table: "Deploys");

            migrationBuilder.DropColumn(
                name: "DeployStageId",
                table: "Deploys");

            migrationBuilder.RenameTable(
                name: "DeployStages",
                newName: "DeployStage");

            migrationBuilder.RenameColumn(
                name: "NextDeployStageId",
                table: "Deploys",
                newName: "StatusId");

            migrationBuilder.RenameColumn(
                name: "NextDeployStageEstimatedDate",
                table: "Deploys",
                newName: "NextEstimatedStatusDate");

            migrationBuilder.RenameColumn(
                name: "DeployStatusId",
                table: "Deploys",
                newName: "NextStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_DeployStages_Status",
                table: "DeployStage",
                newName: "IX_DeployStage_Status");

            migrationBuilder.RenameIndex(
                name: "IX_DeployStages_Sequence",
                table: "DeployStage",
                newName: "IX_DeployStage_Sequence");

            migrationBuilder.AddColumn<int>(
                name: "NextStageId",
                table: "DeployStatus",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StageId",
                table: "DeployStatus",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkingDaysReqdForNextStage",
                table: "DeployStatus",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "CVRefId1",
                table: "Deploys",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeployStage",
                table: "DeployStage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CVRefs_DeployStage_DeployStageId",
                table: "CVRefs",
                column: "DeployStageId",
                principalTable: "DeployStage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Deploys_CVRefs_CVRefId1",
                table: "Deploys",
                column: "CVRefId1",
                principalTable: "CVRefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CVRefs_DeployStage_DeployStageId",
                table: "CVRefs");

            migrationBuilder.DropForeignKey(
                name: "FK_Deploys_CVRefs_CVRefId1",
                table: "Deploys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeployStage",
                table: "DeployStage");

            migrationBuilder.DropColumn(
                name: "NextStageId",
                table: "DeployStatus");

            migrationBuilder.DropColumn(
                name: "StageId",
                table: "DeployStatus");

            migrationBuilder.DropColumn(
                name: "WorkingDaysReqdForNextStage",
                table: "DeployStatus");

            migrationBuilder.RenameTable(
                name: "DeployStage",
                newName: "DeployStages");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "Deploys",
                newName: "NextDeployStageId");

            migrationBuilder.RenameColumn(
                name: "NextStatusId",
                table: "Deploys",
                newName: "DeployStatusId");

            migrationBuilder.RenameColumn(
                name: "NextEstimatedStatusDate",
                table: "Deploys",
                newName: "NextDeployStageEstimatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_DeployStage_Status",
                table: "DeployStages",
                newName: "IX_DeployStages_Status");

            migrationBuilder.RenameIndex(
                name: "IX_DeployStage_Sequence",
                table: "DeployStages",
                newName: "IX_DeployStages_Sequence");

            migrationBuilder.AlterColumn<int>(
                name: "CVRefId1",
                table: "Deploys",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CVRefId2",
                table: "Deploys",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeployStageId",
                table: "Deploys",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeployStages",
                table: "DeployStages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CVRefs_DeployStages_DeployStageId",
                table: "CVRefs",
                column: "DeployStageId",
                principalTable: "DeployStages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Deploys_CVRefs_CVRefId1",
                table: "Deploys",
                column: "CVRefId1",
                principalTable: "CVRefs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
