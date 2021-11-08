using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class EntityAddressesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntityAddress_Candidates_CandidateId",
                table: "EntityAddress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EntityAddress",
                table: "EntityAddress");

            migrationBuilder.RenameTable(
                name: "EntityAddress",
                newName: "EntityAddresses");

            migrationBuilder.RenameIndex(
                name: "IX_EntityAddress_CandidateId",
                table: "EntityAddresses",
                newName: "IX_EntityAddresses_CandidateId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EntityAddresses",
                table: "EntityAddresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EntityAddresses_Candidates_CandidateId",
                table: "EntityAddresses",
                column: "CandidateId",
                principalTable: "Candidates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntityAddresses_Candidates_CandidateId",
                table: "EntityAddresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EntityAddresses",
                table: "EntityAddresses");

            migrationBuilder.RenameTable(
                name: "EntityAddresses",
                newName: "EntityAddress");

            migrationBuilder.RenameIndex(
                name: "IX_EntityAddresses_CandidateId",
                table: "EntityAddress",
                newName: "IX_EntityAddress_CandidateId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EntityAddress",
                table: "EntityAddress",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EntityAddress_Candidates_CandidateId",
                table: "EntityAddress",
                column: "CandidateId",
                principalTable: "Candidates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
