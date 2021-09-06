using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class CVRefAddedColumnCharges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employments_SelectionDecisions_SelectionDecisionId",
                table: "Employments");

            migrationBuilder.DropIndex(
                name: "IX_Employments_SelectionDecisionId",
                table: "Employments");

            migrationBuilder.AddColumn<int>(
                name: "Charges",
                table: "SelectionDecisions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ContractPeriodInMonths",
                table: "SelectionDecisions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FoodAllowance",
                table: "SelectionDecisions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "FoodProvidedFree",
                table: "SelectionDecisions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "HousingAllowance",
                table: "SelectionDecisions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HousingProvidedFree",
                table: "SelectionDecisions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LeaveAirfareEntitlementAfterMonths",
                table: "SelectionDecisions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeavePerYearInDays",
                table: "SelectionDecisions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OtherAllowance",
                table: "SelectionDecisions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Salary",
                table: "SelectionDecisions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SalaryCurrency",
                table: "SelectionDecisions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SelectedOn",
                table: "SelectionDecisions",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TransportAllowance",
                table: "SelectionDecisions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "TransportProvidedFree",
                table: "SelectionDecisions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Charges",
                table: "SelectionDecisions");

            migrationBuilder.DropColumn(
                name: "ContractPeriodInMonths",
                table: "SelectionDecisions");

            migrationBuilder.DropColumn(
                name: "FoodAllowance",
                table: "SelectionDecisions");

            migrationBuilder.DropColumn(
                name: "FoodProvidedFree",
                table: "SelectionDecisions");

            migrationBuilder.DropColumn(
                name: "HousingAllowance",
                table: "SelectionDecisions");

            migrationBuilder.DropColumn(
                name: "HousingProvidedFree",
                table: "SelectionDecisions");

            migrationBuilder.DropColumn(
                name: "LeaveAirfareEntitlementAfterMonths",
                table: "SelectionDecisions");

            migrationBuilder.DropColumn(
                name: "LeavePerYearInDays",
                table: "SelectionDecisions");

            migrationBuilder.DropColumn(
                name: "OtherAllowance",
                table: "SelectionDecisions");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "SelectionDecisions");

            migrationBuilder.DropColumn(
                name: "SalaryCurrency",
                table: "SelectionDecisions");

            migrationBuilder.DropColumn(
                name: "SelectedOn",
                table: "SelectionDecisions");

            migrationBuilder.DropColumn(
                name: "TransportAllowance",
                table: "SelectionDecisions");

            migrationBuilder.DropColumn(
                name: "TransportProvidedFree",
                table: "SelectionDecisions");

            migrationBuilder.CreateIndex(
                name: "IX_Employments_SelectionDecisionId",
                table: "Employments",
                column: "SelectionDecisionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employments_SelectionDecisions_SelectionDecisionId",
                table: "Employments",
                column: "SelectionDecisionId",
                principalTable: "SelectionDecisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
