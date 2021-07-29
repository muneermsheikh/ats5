using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class DeploysAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CVDeploys");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationNo",
                table: "CVRefs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CandidateName",
                table: "CVRefs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "CVRefs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "CVRefs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderNo",
                table: "CVRefs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Deploys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CVRefId = table.Column<int>(type: "INTEGER", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeployStatusId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeployStageId = table.Column<int>(type: "INTEGER", nullable: false),
                    NextDeployStageId = table.Column<int>(type: "INTEGER", nullable: false),
                    NextDeployStageEstimatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CVRefId1 = table.Column<int>(type: "INTEGER", nullable: false),
                    CVRefId2 = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deploys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deploys_CVRefs_CVRefId",
                        column: x => x.CVRefId,
                        principalTable: "CVRefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deploys_CVRefs_CVRefId1",
                        column: x => x.CVRefId1,
                        principalTable: "CVRefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeployStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StatusName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeployStatus", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deploys_CVRefId",
                table: "Deploys",
                column: "CVRefId");

            migrationBuilder.CreateIndex(
                name: "IX_Deploys_CVRefId1",
                table: "Deploys",
                column: "CVRefId1");

            migrationBuilder.CreateIndex(
                name: "IX_DeployStatus_StatusName",
                table: "DeployStatus",
                column: "StatusName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deploys");

            migrationBuilder.DropTable(
                name: "DeployStatus");

            migrationBuilder.DropColumn(
                name: "ApplicationNo",
                table: "CVRefs");

            migrationBuilder.DropColumn(
                name: "CandidateName",
                table: "CVRefs");

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "CVRefs");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "CVRefs");

            migrationBuilder.DropColumn(
                name: "OrderNo",
                table: "CVRefs");

            migrationBuilder.CreateTable(
                name: "CVDeploys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CVRefId = table.Column<int>(type: "INTEGER", nullable: false),
                    CVRefId1 = table.Column<int>(type: "INTEGER", nullable: false),
                    CVRefId2 = table.Column<int>(type: "INTEGER", nullable: false),
                    DeployStageId = table.Column<int>(type: "INTEGER", nullable: false),
                    NextDeployStageEstimatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NextDeployStageId = table.Column<int>(type: "INTEGER", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CVDeploys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CVDeploys_CVRefs_CVRefId",
                        column: x => x.CVRefId,
                        principalTable: "CVRefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CVDeploys_CVRefs_CVRefId1",
                        column: x => x.CVRefId1,
                        principalTable: "CVRefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CVDeploys_CVRefId",
                table: "CVDeploys",
                column: "CVRefId");

            migrationBuilder.CreateIndex(
                name: "IX_CVDeploys_CVRefId1",
                table: "CVDeploys",
                column: "CVRefId1");
        }
    }
}
