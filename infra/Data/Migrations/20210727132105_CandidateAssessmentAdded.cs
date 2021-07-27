using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class CandidateAssessmentAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefStatusId",
                table: "CVRefs");

            migrationBuilder.CreateTable(
                name: "CandidateAssessments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    AssessedById = table.Column<int>(type: "INTEGER", nullable: false),
                    CandidateId = table.Column<int>(type: "INTEGER", nullable: false),
                    AssessedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AssessResult = table.Column<string>(type: "TEXT", nullable: false),
                    Remarks = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateAssessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CandidateAssessments_OrderItems_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CVRefRestrictions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    restrictionReason = table.Column<int>(type: "INTEGER", nullable: false),
                    RestrictedById = table.Column<int>(type: "INTEGER", nullable: false),
                    RestrictedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RestrictionLifted = table.Column<bool>(type: "INTEGER", nullable: false),
                    RestrictionLiftedById = table.Column<int>(type: "INTEGER", nullable: false),
                    RestrictionLiftedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Remarks = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CVRefRestrictions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CandidateAssessmentItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CandidateAssessmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuestionNo = table.Column<int>(type: "INTEGER", nullable: false),
                    AssessmentParameter = table.Column<string>(type: "TEXT", nullable: true),
                    Question = table.Column<string>(type: "TEXT", nullable: true),
                    IsMandatory = table.Column<bool>(type: "INTEGER", nullable: false),
                    Assessed = table.Column<bool>(type: "INTEGER", nullable: false),
                    MaxPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    Points = table.Column<int>(type: "INTEGER", nullable: false),
                    Remarks = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateAssessmentItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CandidateAssessmentItems_CandidateAssessments_CandidateAssessmentId",
                        column: x => x.CandidateAssessmentId,
                        principalTable: "CandidateAssessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CandidateAssessmentItems_CandidateAssessmentId",
                table: "CandidateAssessmentItems",
                column: "CandidateAssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateAssessments_CandidateId_OrderItemId",
                table: "CandidateAssessments",
                columns: new[] { "CandidateId", "OrderItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CandidateAssessments_OrderItemId",
                table: "CandidateAssessments",
                column: "OrderItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CandidateAssessmentItems");

            migrationBuilder.DropTable(
                name: "CVRefRestrictions");

            migrationBuilder.DropTable(
                name: "CandidateAssessments");

            migrationBuilder.AddColumn<int>(
                name: "RefStatusId",
                table: "CVRefs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
