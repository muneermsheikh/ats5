using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class UserHistoryRedesigned : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CandidateId",
                table: "UserHistories");

            migrationBuilder.RenameColumn(
                name: "PartyName",
                table: "UserHistories",
                newName: "PersonType");

            migrationBuilder.RenameColumn(
                name: "CustomerOfficialId",
                table: "UserHistories",
                newName: "PersonId");

            migrationBuilder.RenameColumn(
                name: "AadharNo",
                table: "UserHistories",
                newName: "PersonName");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationNo",
                table: "UserHistories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "loggedInEmployeeId",
                table: "AppUser",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "loggedInEmployeeId",
                table: "AppUser");

            migrationBuilder.RenameColumn(
                name: "PersonType",
                table: "UserHistories",
                newName: "PartyName");

            migrationBuilder.RenameColumn(
                name: "PersonName",
                table: "UserHistories",
                newName: "AadharNo");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "UserHistories",
                newName: "CustomerOfficialId");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationNo",
                table: "UserHistories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CandidateId",
                table: "UserHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
