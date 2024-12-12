using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AttendeeCountInEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttendeeCount",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttendeeCount",
                table: "Events");
        }
    }
}
