using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeHive.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Contract_Price : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Contracts",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Contracts");
        }
    }
}
