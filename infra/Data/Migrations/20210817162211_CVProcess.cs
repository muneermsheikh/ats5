using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class CVProcess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TaskStatus",
                table: "Tasks",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TaskDescription",
                table: "Tasks",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedOn",
                table: "Tasks",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PostTaskAction",
                table: "Tasks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "NoReviewBySupervisor",
                table: "OrderItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "OrderItemAssessmentQs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "HRMProcessings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HRMgrId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReviewResultId = table.Column<int>(type: "INTEGER", nullable: false),
                    HRMTaskId = table.Column<int>(type: "INTEGER", nullable: false),
                    HRMReviewedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HRMRemarks = table.Column<string>(type: "TEXT", nullable: true),
                    DocControllerAdminTaskId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRMProcessings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HRSupProcessings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HRSupId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReviewedByHRSupOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReviewResultId = table.Column<int>(type: "INTEGER", nullable: false),
                    SupTaskId = table.Column<int>(type: "INTEGER", nullable: false),
                    HRSupRemarks = table.Column<string>(type: "TEXT", nullable: true),
                    HRMProcessingId = table.Column<int>(type: "INTEGER", nullable: false),
                    HRMId = table.Column<int>(type: "INTEGER", nullable: false),
                    DocControllerAdminTaskId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRSupProcessings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HRSupProcessings_HRMProcessings_HRMProcessingId",
                        column: x => x.HRMProcessingId,
                        principalTable: "HRMProcessings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HRCVProcessings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CandidateId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    HRExecutiveId = table.Column<int>(type: "INTEGER", nullable: false),
                    HRExecTaskId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubmittedByHRExecOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HRExecRemarks = table.Column<string>(type: "TEXT", nullable: true),
                    HRSupId = table.Column<int>(type: "INTEGER", nullable: false),
                    HRSupProcessingId = table.Column<int>(type: "INTEGER", nullable: false),
                    DocControllerAdminTaskId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRCVProcessings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HRCVProcessings_HRSupProcessings_HRSupProcessingId",
                        column: x => x.HRSupProcessingId,
                        principalTable: "HRSupProcessings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HRCVProcessings_CandidateId_OrderItemId",
                table: "HRCVProcessings",
                columns: new[] { "CandidateId", "OrderItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HRCVProcessings_HRSupProcessingId",
                table: "HRCVProcessings",
                column: "HRSupProcessingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HRMProcessings_HRMTaskId",
                table: "HRMProcessings",
                column: "HRMTaskId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HRSupProcessings_HRMProcessingId",
                table: "HRSupProcessings",
                column: "HRMProcessingId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HRCVProcessings");

            migrationBuilder.DropTable(
                name: "HRSupProcessings");

            migrationBuilder.DropTable(
                name: "HRMProcessings");

            migrationBuilder.DropColumn(
                name: "CompletedOn",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "PostTaskAction",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "NoReviewBySupervisor",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "OrderItemAssessmentQs");

            migrationBuilder.AlterColumn<string>(
                name: "TaskStatus",
                table: "Tasks",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "TaskDescription",
                table: "Tasks",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
