using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class _10Aug21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractReviews_ReviewStatuses_ReviewStatusId",
                table: "ContractReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskItems_Tasks_TaskId",
                table: "TaskItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskItems_Tasks_TaskId1",
                table: "TaskItems");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_TaskType",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_TaskItems_TaskId",
                table: "TaskItems");

            migrationBuilder.DropIndex(
                name: "IX_ContractReviews_ReviewStatusId",
                table: "ContractReviews");

            migrationBuilder.DropColumn(
                name: "TaskType",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "MessageType",
                table: "EmailMessages");

            migrationBuilder.RenameColumn(
                name: "TaskId1",
                table: "TaskItems",
                newName: "ApplicationTaskId1");

            migrationBuilder.RenameColumn(
                name: "TaskId",
                table: "TaskItems",
                newName: "TaskType");

            migrationBuilder.RenameIndex(
                name: "IX_TaskItems_TaskId1",
                table: "TaskItems",
                newName: "IX_TaskItems_ApplicationTaskId1");

            migrationBuilder.RenameColumn(
                name: "ReviewDescription",
                table: "ReviewItemDatas",
                newName: "ReviewParameter");

            migrationBuilder.AddColumn<int>(
                name: "TaskTypeId",
                table: "Tasks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TaskItems",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationTaskId",
                table: "TaskItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CandidateId",
                table: "TaskItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "TaskItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "TaskItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderItemId",
                table: "TaskItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderNo",
                table: "TaskItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "TaskItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsMandatoryTrue",
                table: "ReviewItemDatas",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Response",
                table: "ReviewItemDatas",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FoodNotProvided",
                table: "Remunerations",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HousingNotProvided",
                table: "Remunerations",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TransportNotProvided",
                table: "Remunerations",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "WorkHours",
                table: "Remunerations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderRefDate",
                table: "Orders",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ProjectManagerId",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderNo",
                table: "OrderItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "RequireInternalReview",
                table: "OrderItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "Employees",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsOfficial",
                table: "EmployeePhones",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MessageTypeId",
                table: "EmailMessages",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Divn",
                table: "CustomerOfficials",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReviewedOn",
                table: "ContractReviews",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "MessageTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReviewItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ContractReviewItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    SrNo = table.Column<int>(type: "INTEGER", nullable: false),
                    ReviewParameter = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    Response = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsMandatoryTrue = table.Column<bool>(type: "INTEGER", nullable: false),
                    Remarks = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewItems_ContractReviewItems_ContractReviewItemId",
                        column: x => x.ContractReviewItemId,
                        principalTable: "ContractReviewItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskTypeId",
                table: "Tasks",
                column: "TaskTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_ApplicationTaskId",
                table: "TaskItems",
                column: "ApplicationTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewItems_ContractReviewItemId_ReviewParameter",
                table: "ReviewItems",
                columns: new[] { "ContractReviewItemId", "ReviewParameter" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReviewItems_ContractReviewItemId_SrNo",
                table: "ReviewItems",
                columns: new[] { "ContractReviewItemId", "SrNo" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItems_Tasks_ApplicationTaskId",
                table: "TaskItems",
                column: "ApplicationTaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItems_Tasks_ApplicationTaskId1",
                table: "TaskItems",
                column: "ApplicationTaskId1",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskItems_Tasks_ApplicationTaskId",
                table: "TaskItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskItems_Tasks_ApplicationTaskId1",
                table: "TaskItems");

            migrationBuilder.DropTable(
                name: "MessageTypes");

            migrationBuilder.DropTable(
                name: "ReviewItems");

            migrationBuilder.DropTable(
                name: "TaskTypes");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_TaskTypeId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_TaskItems_ApplicationTaskId",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "TaskTypeId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ApplicationTaskId",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "CandidateId",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "OrderItemId",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "OrderNo",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "IsMandatoryTrue",
                table: "ReviewItemDatas");

            migrationBuilder.DropColumn(
                name: "Response",
                table: "ReviewItemDatas");

            migrationBuilder.DropColumn(
                name: "FoodNotProvided",
                table: "Remunerations");

            migrationBuilder.DropColumn(
                name: "HousingNotProvided",
                table: "Remunerations");

            migrationBuilder.DropColumn(
                name: "TransportNotProvided",
                table: "Remunerations");

            migrationBuilder.DropColumn(
                name: "WorkHours",
                table: "Remunerations");

            migrationBuilder.DropColumn(
                name: "OrderRefDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProjectManagerId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderNo",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "RequireInternalReview",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "IsOfficial",
                table: "EmployeePhones");

            migrationBuilder.DropColumn(
                name: "MessageTypeId",
                table: "EmailMessages");

            migrationBuilder.DropColumn(
                name: "Divn",
                table: "CustomerOfficials");

            migrationBuilder.RenameColumn(
                name: "TaskType",
                table: "TaskItems",
                newName: "TaskId");

            migrationBuilder.RenameColumn(
                name: "ApplicationTaskId1",
                table: "TaskItems",
                newName: "TaskId1");

            migrationBuilder.RenameIndex(
                name: "IX_TaskItems_ApplicationTaskId1",
                table: "TaskItems",
                newName: "IX_TaskItems_TaskId1");

            migrationBuilder.RenameColumn(
                name: "ReviewParameter",
                table: "ReviewItemDatas",
                newName: "ReviewDescription");

            migrationBuilder.AddColumn<string>(
                name: "TaskType",
                table: "Tasks",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "TaskItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "MessageType",
                table: "EmailMessages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReviewedOn",
                table: "ContractReviews",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskType",
                table: "Tasks",
                column: "TaskType");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_TaskId",
                table: "TaskItems",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractReviews_ReviewStatusId",
                table: "ContractReviews",
                column: "ReviewStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractReviews_ReviewStatuses_ReviewStatusId",
                table: "ContractReviews",
                column: "ReviewStatusId",
                principalTable: "ReviewStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItems_Tasks_TaskId",
                table: "TaskItems",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItems_Tasks_TaskId1",
                table: "TaskItems",
                column: "TaskId1",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
