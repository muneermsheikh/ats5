using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class InterviewItemCandidateFollowupAttendanceStatusIdAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttendanceStatusId",
                table: "InterviewItemCandidatesFollowup",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "FollowupConcluded",
                table: "InterviewItemCandidatesFollowup",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttendanceStatusId",
                table: "InterviewItemCandidatesFollowup");

            migrationBuilder.DropColumn(
                name: "FollowupConcluded",
                table: "InterviewItemCandidatesFollowup");
        }
    }
}
