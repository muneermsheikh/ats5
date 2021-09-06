using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class CandidateECNRAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmigProcessInchargeId",
                table: "Orders",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MedicalProcessInchargeEmpId",
                table: "Orders",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TravelProcessInchargeId",
                table: "Orders",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VisaProcessInchargeEmpId",
                table: "Orders",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsProcessingOnly",
                table: "OrderItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ProcessName",
                table: "DeployStatus",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Ecnr",
                table: "CVReviews",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Ecnr",
                table: "CVRefs",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Ecnr",
                table: "Candidates",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmigProcessInchargeId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "MedicalProcessInchargeEmpId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TravelProcessInchargeId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "VisaProcessInchargeEmpId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsProcessingOnly",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ProcessName",
                table: "DeployStatus");

            migrationBuilder.DropColumn(
                name: "Ecnr",
                table: "CVReviews");

            migrationBuilder.DropColumn(
                name: "Ecnr",
                table: "CVRefs");

            migrationBuilder.DropColumn(
                name: "Ecnr",
                table: "Candidates");
        }
    }
}
