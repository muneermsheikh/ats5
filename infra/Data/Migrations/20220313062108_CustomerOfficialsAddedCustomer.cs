using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class CustomerOfficialsAddedCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CandidateId1",
                table: "UserPhones",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerId1",
                table: "CustomerOfficials",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPhones_CandidateId1",
                table: "UserPhones",
                column: "CandidateId1");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOfficials_CustomerId1",
                table: "CustomerOfficials",
                column: "CustomerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerOfficials_Customers_CustomerId1",
                table: "CustomerOfficials",
                column: "CustomerId1",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPhones_Candidates_CandidateId1",
                table: "UserPhones",
                column: "CandidateId1",
                principalTable: "Candidates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerOfficials_Customers_CustomerId1",
                table: "CustomerOfficials");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPhones_Candidates_CandidateId1",
                table: "UserPhones");

            migrationBuilder.DropIndex(
                name: "IX_UserPhones_CandidateId1",
                table: "UserPhones");

            migrationBuilder.DropIndex(
                name: "IX_CustomerOfficials_CustomerId1",
                table: "CustomerOfficials");

            migrationBuilder.DropColumn(
                name: "CandidateId1",
                table: "UserPhones");

            migrationBuilder.DropColumn(
                name: "CustomerId1",
                table: "CustomerOfficials");
        }
    }
}
