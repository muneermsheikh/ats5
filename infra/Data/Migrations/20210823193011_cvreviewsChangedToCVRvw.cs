using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class cvreviewsChangedToCVRvw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CVReviews_CVReviewBySups_CVReviewBySupId",
                table: "CVReviews");

            migrationBuilder.DropTable(
                name: "CVReviewBySups");

            migrationBuilder.DropTable(
                name: "CVReviewByHRMs");

            migrationBuilder.DropIndex(
                name: "IX_CVReviews_CVReviewBySupId",
                table: "CVReviews");

            migrationBuilder.DropColumn(
                name: "CVReviewBySupId",
                table: "CVReviews");

            migrationBuilder.AddColumn<int>(
                name: "HRMId",
                table: "CVReviews",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "HRMRemarks",
                table: "CVReviews",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HRMReviewResultId",
                table: "CVReviews",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "HRMReviewedOn",
                table: "CVReviews",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "HRMTaskId",
                table: "CVReviews",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReviewedBySupOn",
                table: "CVReviews",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SupRemarks",
                table: "CVReviews",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SupReviewResultId",
                table: "CVReviews",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SupTaskId",
                table: "CVReviews",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HRMId",
                table: "CVReviews");

            migrationBuilder.DropColumn(
                name: "HRMRemarks",
                table: "CVReviews");

            migrationBuilder.DropColumn(
                name: "HRMReviewResultId",
                table: "CVReviews");

            migrationBuilder.DropColumn(
                name: "HRMReviewedOn",
                table: "CVReviews");

            migrationBuilder.DropColumn(
                name: "HRMTaskId",
                table: "CVReviews");

            migrationBuilder.DropColumn(
                name: "ReviewedBySupOn",
                table: "CVReviews");

            migrationBuilder.DropColumn(
                name: "SupRemarks",
                table: "CVReviews");

            migrationBuilder.DropColumn(
                name: "SupReviewResultId",
                table: "CVReviews");

            migrationBuilder.DropColumn(
                name: "SupTaskId",
                table: "CVReviews");

            migrationBuilder.AddColumn<int>(
                name: "CVReviewBySupId",
                table: "CVReviews",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CVReviewByHRMs",
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
                    table.PrimaryKey("PK_CVReviewByHRMs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CVReviewBySups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CVReviewbyHRMId = table.Column<int>(type: "INTEGER", nullable: false),
                    DocControllerAdminTaskId = table.Column<int>(type: "INTEGER", nullable: false),
                    HRMId = table.Column<int>(type: "INTEGER", nullable: false),
                    HRSupId = table.Column<int>(type: "INTEGER", nullable: false),
                    HRSupRemarks = table.Column<string>(type: "TEXT", nullable: true),
                    ReviewResultId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReviewedByHRSupOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SupTaskId = table.Column<int>(type: "INTEGER", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_CVReviews_CVReviewBySupId",
                table: "CVReviews",
                column: "CVReviewBySupId",
                unique: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_CVReviews_CVReviewBySups_CVReviewBySupId",
                table: "CVReviews",
                column: "CVReviewBySupId",
                principalTable: "CVReviewBySups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
