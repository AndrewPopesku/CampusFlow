using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusFlow.Migrations
{
    /// <inheritdoc />
    public partial class CreatedScheduleDatetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Schedules_ScheduleId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Attendances");

            migrationBuilder.RenameColumn(
                name: "ScheduleId",
                table: "Attendances",
                newName: "ScheduleDateId");

            migrationBuilder.RenameIndex(
                name: "IX_Attendances_ScheduleId",
                table: "Attendances",
                newName: "IX_Attendances_ScheduleDateId");

            migrationBuilder.CreateTable(
                name: "ScheduleDate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleDate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleDate_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleDate_ScheduleId",
                table: "ScheduleDate",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_ScheduleDate_ScheduleDateId",
                table: "Attendances",
                column: "ScheduleDateId",
                principalTable: "ScheduleDate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_ScheduleDate_ScheduleDateId",
                table: "Attendances");

            migrationBuilder.DropTable(
                name: "ScheduleDate");

            migrationBuilder.RenameColumn(
                name: "ScheduleDateId",
                table: "Attendances",
                newName: "ScheduleId");

            migrationBuilder.RenameIndex(
                name: "IX_Attendances_ScheduleDateId",
                table: "Attendances",
                newName: "IX_Attendances_ScheduleId");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Attendances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Schedules_ScheduleId",
                table: "Attendances",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
