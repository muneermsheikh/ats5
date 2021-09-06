﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace infra.Data.Migrations
{
    public partial class TaskIndexAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssignedToId_CandidateId_TaskTypeId",
                table: "Tasks",
                columns: new[] { "AssignedToId", "CandidateId", "TaskTypeId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tasks_AssignedToId_CandidateId_TaskTypeId",
                table: "Tasks");
        }
    }
}