using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class AssessmentQBank : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AssessmentQsBank",
                table: "AssessmentQsBank");

            migrationBuilder.DropIndex(
                name: "IX_AssessmentQsBank_QNo_CategoryId",
                table: "AssessmentQsBank");

            migrationBuilder.DropIndex(
                name: "IX_AssessmentQsBank_Question_CategoryId",
                table: "AssessmentQsBank");

            migrationBuilder.DropColumn(
                name: "IsStandardQ",
                table: "AssessmentQsBank");

            migrationBuilder.DropColumn(
                name: "MaxPoints",
                table: "AssessmentQsBank");

            migrationBuilder.DropColumn(
                name: "QNo",
                table: "AssessmentQsBank");

            migrationBuilder.DropColumn(
                name: "Question",
                table: "AssessmentQsBank");

            migrationBuilder.RenameTable(
                name: "AssessmentQsBank",
                newName: "AssessmentQBank");

            migrationBuilder.RenameColumn(
                name: "AssessmentParameter",
                table: "AssessmentQBank",
                newName: "CategoryName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssessmentQBank",
                table: "AssessmentQBank",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AssessmentQBankItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentQBankId = table.Column<int>(type: "int", nullable: false),
                    AssessmentParameter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QNo = table.Column<int>(type: "int", nullable: false),
                    IsStandardQ = table.Column<bool>(type: "bit", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxPoints = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentQBankItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentQBankItems_AssessmentQBank_AssessmentQBankId",
                        column: x => x.AssessmentQBankId,
                        principalTable: "AssessmentQBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentQBank_CategoryId",
                table: "AssessmentQBank",
                column: "CategoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentQBankItems_AssessmentQBankId_QNo",
                table: "AssessmentQBankItems",
                columns: new[] { "AssessmentQBankId", "QNo" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssessmentQBankItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AssessmentQBank",
                table: "AssessmentQBank");

            migrationBuilder.DropIndex(
                name: "IX_AssessmentQBank_CategoryId",
                table: "AssessmentQBank");

            migrationBuilder.RenameTable(
                name: "AssessmentQBank",
                newName: "AssessmentQsBank");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "AssessmentQsBank",
                newName: "AssessmentParameter");

            migrationBuilder.AddColumn<bool>(
                name: "IsStandardQ",
                table: "AssessmentQsBank",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MaxPoints",
                table: "AssessmentQsBank",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QNo",
                table: "AssessmentQsBank",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Question",
                table: "AssessmentQsBank",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AssessmentQsBank",
                table: "AssessmentQsBank",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentQsBank_QNo_CategoryId",
                table: "AssessmentQsBank",
                columns: new[] { "QNo", "CategoryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentQsBank_Question_CategoryId",
                table: "AssessmentQsBank",
                columns: new[] { "Question", "CategoryId" },
                unique: true);
        }
    }
}
