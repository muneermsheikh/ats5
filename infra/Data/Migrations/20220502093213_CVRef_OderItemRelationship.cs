using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class CVRef_OderItemRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CVRefs_OrderItems_OrderItemId",
                table: "CVRefs");

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
                name: "TransportAllowance",
                table: "SelectionDecisions");

            migrationBuilder.DropColumn(
                name: "TransportProvidedFree",
                table: "SelectionDecisions");

            migrationBuilder.AddColumn<DateTime>(
                name: "RefStatusDate",
                table: "CVRefs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Employments_SelectionDecisionId",
                table: "Employments",
                column: "SelectionDecisionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CVRefs_Candidates_CandidateId",
                table: "CVRefs",
                column: "CandidateId",
                principalTable: "Candidates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CVRefs_OrderItems_OrderItemId",
                table: "CVRefs",
                column: "OrderItemId",
                principalTable: "OrderItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employments_SelectionDecisions_SelectionDecisionId",
                table: "Employments",
                column: "SelectionDecisionId",
                principalTable: "SelectionDecisions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CVRefs_Candidates_CandidateId",
                table: "CVRefs");

            migrationBuilder.DropForeignKey(
                name: "FK_CVRefs_OrderItems_OrderItemId",
                table: "CVRefs");

            migrationBuilder.DropForeignKey(
                name: "FK_Employments_SelectionDecisions_SelectionDecisionId",
                table: "Employments");

            migrationBuilder.DropIndex(
                name: "IX_Employments_SelectionDecisionId",
                table: "Employments");

            migrationBuilder.DropColumn(
                name: "RefStatusDate",
                table: "CVRefs");

            migrationBuilder.AddColumn<int>(
                name: "ContractPeriodInMonths",
                table: "SelectionDecisions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FoodAllowance",
                table: "SelectionDecisions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "FoodProvidedFree",
                table: "SelectionDecisions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "HousingAllowance",
                table: "SelectionDecisions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HousingProvidedFree",
                table: "SelectionDecisions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LeaveAirfareEntitlementAfterMonths",
                table: "SelectionDecisions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeavePerYearInDays",
                table: "SelectionDecisions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OtherAllowance",
                table: "SelectionDecisions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Salary",
                table: "SelectionDecisions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SalaryCurrency",
                table: "SelectionDecisions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransportAllowance",
                table: "SelectionDecisions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "TransportProvidedFree",
                table: "SelectionDecisions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_CVRefs_OrderItems_OrderItemId",
                table: "CVRefs",
                column: "OrderItemId",
                principalTable: "OrderItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
