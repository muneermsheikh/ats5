using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class UserPhoneCandidateNaviationRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPhones_Candidates_CandidateId1",
                table: "UserPhones");

            migrationBuilder.DropIndex(
                name: "IX_UserPhones_CandidateId1",
                table: "UserPhones");

            migrationBuilder.DropColumn(
                name: "CandidateId1",
                table: "UserPhones");

            migrationBuilder.AlterColumn<string>(
                name: "MobileNo",
                table: "UserPhones",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MobileNo",
                table: "UserPhones",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AddColumn<int>(
                name: "CandidateId1",
                table: "UserPhones",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPhones_CandidateId1",
                table: "UserPhones",
                column: "CandidateId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPhones_Candidates_CandidateId1",
                table: "UserPhones",
                column: "CandidateId1",
                principalTable: "Candidates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
