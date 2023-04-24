using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusFlow.Migrations
{
    /// <inheritdoc />
    public partial class NewTimeslotTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimeSlotId",
                table: "Schedules",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TimeSlot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassNumber = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSlot", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_TimeSlotId",
                table: "Schedules",
                column: "TimeSlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_TimeSlot_TimeSlotId",
                table: "Schedules",
                column: "TimeSlotId",
                principalTable: "TimeSlot",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_TimeSlot_TimeSlotId",
                table: "Schedules");

            migrationBuilder.DropTable(
                name: "TimeSlot");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_TimeSlotId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "TimeSlotId",
                table: "Schedules");
        }
    }
}
