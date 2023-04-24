using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusFlow.Migrations
{
    /// <inheritdoc />
    public partial class AddedPropSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_TimeSlot_TimeSlotId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_TimeSlotId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "TimeSlotId",
                table: "Subjects");

            migrationBuilder.AddColumn<int>(
                name: "ClassType",
                table: "Schedules",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassType",
                table: "Schedules");

            migrationBuilder.AddColumn<int>(
                name: "TimeSlotId",
                table: "Subjects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_TimeSlotId",
                table: "Subjects",
                column: "TimeSlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_TimeSlot_TimeSlotId",
                table: "Subjects",
                column: "TimeSlotId",
                principalTable: "TimeSlot",
                principalColumn: "TimeSlotId");
        }
    }
}
