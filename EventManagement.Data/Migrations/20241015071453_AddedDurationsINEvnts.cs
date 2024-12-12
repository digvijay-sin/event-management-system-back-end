using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedDurationsINEvnts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DurationDays",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DurationHours",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationDays",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "DurationHours",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Events");
        }
    }
}
