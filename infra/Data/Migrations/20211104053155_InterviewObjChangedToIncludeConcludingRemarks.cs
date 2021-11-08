using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class InterviewObjChangedToIncludeConcludingRemarks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Remarks",
                table: "InterviewItemCandidates",
                newName: "ConcludingRemarks");

            migrationBuilder.AddColumn<string>(
                name: "ConcludingRemarks",
                table: "Interviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcludingRemarks",
                table: "InterviewItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcludingRemarks",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "ConcludingRemarks",
                table: "InterviewItems");

            migrationBuilder.RenameColumn(
                name: "ConcludingRemarks",
                table: "InterviewItemCandidates",
                newName: "Remarks");
        }
    }
}
