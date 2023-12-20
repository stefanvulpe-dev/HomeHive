using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeHive.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Deleted_SizeCapacity_FromRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Rooms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Rooms",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "Rooms",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
