using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeHive.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_UtilityType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UtilityName",
                table: "Utilities");

            migrationBuilder.AddColumn<string>(
                name: "UtilityType",
                table: "Utilities",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UtilityType",
                table: "Utilities");

            migrationBuilder.AddColumn<string>(
                name: "UtilityName",
                table: "Utilities",
                type: "text",
                nullable: true);
        }
    }
}
