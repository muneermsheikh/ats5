using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class orderitemsstatusIsInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckListItemHRs");

            migrationBuilder.CreateTable(
                name: "ChecklistHRItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChecklistHRId = table.Column<int>(type: "int", nullable: false),
                    SrNo = table.Column<int>(type: "int", nullable: false),
                    Parameter = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Accepts = table.Column<bool>(type: "bit", nullable: false),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exceptions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MandatoryTrue = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistHRItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChecklistHRItems_ChecklistHRs_ChecklistHRId",
                        column: x => x.ChecklistHRId,
                        principalTable: "ChecklistHRs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistHRItems_ChecklistHRId",
                table: "ChecklistHRItems",
                column: "ChecklistHRId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChecklistHRItems");

            migrationBuilder.CreateTable(
                name: "CheckListItemHRs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Accepts = table.Column<bool>(type: "bit", nullable: false),
                    ChecklistHRId = table.Column<int>(type: "int", nullable: false),
                    Exceptions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MandatoryTrue = table.Column<bool>(type: "bit", nullable: false),
                    Parameter = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SrNo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckListItemHRs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckListItemHRs_ChecklistHRs_ChecklistHRId",
                        column: x => x.ChecklistHRId,
                        principalTable: "ChecklistHRs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckListItemHRs_ChecklistHRId",
                table: "CheckListItemHRs",
                column: "ChecklistHRId");
        }
    }
}
