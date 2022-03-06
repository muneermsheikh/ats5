using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class DLForwards : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DLForwards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderItemId = table.Column<int>(type: "int", nullable: false),
                    CategoryRefAndName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerOfficialId = table.Column<int>(type: "int", nullable: false),
                    DateForwarded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmailIdForwardedTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNoForwardedTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WhatsAppNoForwardedTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoggedInAppUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DLForwards", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DLForwards");
        }
    }
}
