using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeHive.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Contract_Status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Contracts",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Contracts");
        }
    }
}
