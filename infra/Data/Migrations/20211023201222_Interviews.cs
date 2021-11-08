using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class Interviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNo",
                table: "UserPhones");

            migrationBuilder.AlterColumn<string>(
                name: "MobileNo",
                table: "UserPhones",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "UserPhones",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InterviewAttendancesStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewAttendancesStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Interviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InterviewMode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InterviewerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InterviewVenue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InterviewDateFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InterviewDateUpto = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InterviewLeaderId = table.Column<int>(type: "int", nullable: false),
                    CustomerRepresentative = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InterviewStatus = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersContacted",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CandidateId = table.Column<int>(type: "int", nullable: false),
                    OrderItemId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    IsValid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersContacted", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InterviewItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InterviewId = table.Column<int>(type: "int", nullable: false),
                    OrderItemId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    InterviewDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InterviewMode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InterviewerName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterviewItems_Interviews_InterviewId",
                        column: x => x.InterviewId,
                        principalTable: "Interviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserContactedItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserContactedId = table.Column<int>(type: "int", nullable: false),
                    DateOfContact = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoggedInUserId = table.Column<int>(type: "int", nullable: false),
                    enumContactResult = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NextReminderOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContactedItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserContactedItems_UsersContacted_UserContactedId",
                        column: x => x.UserContactedId,
                        principalTable: "UsersContacted",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterviewItemCandidates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InterviewItemId = table.Column<int>(type: "int", nullable: false),
                    CandidateId = table.Column<int>(type: "int", nullable: false),
                    CandidateName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationNo = table.Column<int>(type: "int", nullable: false),
                    PassportNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScheduledFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduledUpto = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InterviewMode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InterviewedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AttendanceStatusId = table.Column<int>(type: "int", nullable: false),
                    SelectionStatusId = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewItemCandidates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterviewItemCandidates_InterviewItems_InterviewItemId",
                        column: x => x.InterviewItemId,
                        principalTable: "InterviewItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterviewItemCandidatesFollowup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InterviewItemCandidateId = table.Column<int>(type: "int", nullable: false),
                    ContactedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContactedById = table.Column<int>(type: "int", nullable: false),
                    MobileNoCalled = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewItemCandidatesFollowup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterviewItemCandidatesFollowup_InterviewItemCandidates_InterviewItemCandidateId",
                        column: x => x.InterviewItemCandidateId,
                        principalTable: "InterviewItemCandidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InterviewAttendancesStatus_Status",
                table: "InterviewAttendancesStatus",
                column: "Status",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InterviewItemCandidates_InterviewItemId",
                table: "InterviewItemCandidates",
                column: "InterviewItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewItemCandidatesFollowup_InterviewItemCandidateId",
                table: "InterviewItemCandidatesFollowup",
                column: "InterviewItemCandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_InterviewItems_InterviewId",
                table: "InterviewItems",
                column: "InterviewId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContactedItems_UserContactedId",
                table: "UserContactedItems",
                column: "UserContactedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterviewAttendancesStatus");

            migrationBuilder.DropTable(
                name: "InterviewItemCandidatesFollowup");

            migrationBuilder.DropTable(
                name: "UserContactedItems");

            migrationBuilder.DropTable(
                name: "InterviewItemCandidates");

            migrationBuilder.DropTable(
                name: "UsersContacted");

            migrationBuilder.DropTable(
                name: "InterviewItems");

            migrationBuilder.DropTable(
                name: "Interviews");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "UserPhones");

            migrationBuilder.AlterColumn<string>(
                name: "MobileNo",
                table: "UserPhones",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNo",
                table: "UserPhones",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }
    }
}
