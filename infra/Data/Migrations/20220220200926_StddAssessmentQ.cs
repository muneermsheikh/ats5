using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class StddAssessmentQ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssessmentStandardQs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentParameter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QNo = table.Column<int>(type: "int", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaxPoints = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentStandardQs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentStandardQs_QNo",
                table: "AssessmentStandardQs",
                column: "QNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentStandardQs_Question",
                table: "AssessmentStandardQs",
                column: "Question",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssessmentStandardQs");
        }
    }
}
