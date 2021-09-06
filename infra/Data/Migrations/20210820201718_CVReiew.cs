using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class CVReiew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HRCVProcessings");

            migrationBuilder.DropTable(
                name: "HRSupProcessings");

            migrationBuilder.DropTable(
                name: "HRMProcessings");

            migrationBuilder.CreateTable(
                name: "CVReviewByHRMs",
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
                    table.PrimaryKey("PK_CVReviewByHRMs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CVReviewBySups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HRSupId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReviewedByHRSupOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReviewResultId = table.Column<int>(type: "INTEGER", nullable: false),
                    SupTaskId = table.Column<int>(type: "INTEGER", nullable: false),
                    HRSupRemarks = table.Column<string>(type: "TEXT", nullable: true),
                    CVReviewbyHRMId = table.Column<int>(type: "INTEGER", nullable: false),
                    HRMId = table.Column<int>(type: "INTEGER", nullable: false),
                    DocControllerAdminTaskId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CVReviewBySups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CVReviewBySups_CVReviewByHRMs_CVReviewbyHRMId",
                        column: x => x.CVReviewbyHRMId,
                        principalTable: "CVReviewByHRMs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CVReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChecklistHRId = table.Column<int>(type: "INTEGER", nullable: true),
                    CandidateId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    HRExecutiveId = table.Column<int>(type: "INTEGER", nullable: false),
                    HRExecTaskId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubmittedByHRExecOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HRExecRemarks = table.Column<string>(type: "TEXT", nullable: true),
                    HRSupId = table.Column<int>(type: "INTEGER", nullable: false),
                    CVReviewBySupId = table.Column<int>(type: "INTEGER", nullable: true),
                    DocControllerAdminTaskId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CVReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CVReviews_CVReviewBySups_CVReviewBySupId",
                        column: x => x.CVReviewBySupId,
                        principalTable: "CVReviewBySups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CVReviewByHRMs_HRMTaskId",
                table: "CVReviewByHRMs",
                column: "HRMTaskId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CVReviewBySups_CVReviewbyHRMId",
                table: "CVReviewBySups",
                column: "CVReviewbyHRMId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CVReviews_CandidateId_OrderItemId",
                table: "CVReviews",
                columns: new[] { "CandidateId", "OrderItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CVReviews_CVReviewBySupId",
                table: "CVReviews",
                column: "CVReviewBySupId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CVReviews");

            migrationBuilder.DropTable(
                name: "CVReviewBySups");

            migrationBuilder.DropTable(
                name: "CVReviewByHRMs");

            migrationBuilder.CreateTable(
                name: "HRMProcessings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DocControllerAdminTaskId = table.Column<int>(type: "INTEGER", nullable: false),
                    HRMRemarks = table.Column<string>(type: "TEXT", nullable: true),
                    HRMReviewedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HRMTaskId = table.Column<int>(type: "INTEGER", nullable: false),
                    HRMgrId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReviewResultId = table.Column<int>(type: "INTEGER", nullable: false)
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
                    DocControllerAdminTaskId = table.Column<int>(type: "INTEGER", nullable: false),
                    HRMId = table.Column<int>(type: "INTEGER", nullable: false),
                    HRMProcessingId = table.Column<int>(type: "INTEGER", nullable: false),
                    HRSupId = table.Column<int>(type: "INTEGER", nullable: false),
                    HRSupRemarks = table.Column<string>(type: "TEXT", nullable: true),
                    ReviewResultId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReviewedByHRSupOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SupTaskId = table.Column<int>(type: "INTEGER", nullable: false)
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
                    DocControllerAdminTaskId = table.Column<int>(type: "INTEGER", nullable: false),
                    HRExecRemarks = table.Column<string>(type: "TEXT", nullable: true),
                    HRExecTaskId = table.Column<int>(type: "INTEGER", nullable: false),
                    HRExecutiveId = table.Column<int>(type: "INTEGER", nullable: false),
                    HRSupId = table.Column<int>(type: "INTEGER", nullable: false),
                    HRSupProcessingId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubmittedByHRExecOn = table.Column<DateTime>(type: "TEXT", nullable: false)
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
    }
}
