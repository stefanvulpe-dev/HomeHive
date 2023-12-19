using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeHive.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateCustom_ManyToMany_Estate_Rooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Estates_EstateId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_EstateId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Rooms");

            migrationBuilder.AlterColumn<string>(
                name: "ContractType",
                table: "Contracts",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "EstateRoom",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EstateId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstateRoom", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstateRoom_Estates_EstateId",
                        column: x => x.EstateId,
                        principalTable: "Estates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstateRoom_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EstateRoom_EstateId",
                table: "EstateRoom",
                column: "EstateId");

            migrationBuilder.CreateIndex(
                name: "IX_EstateRoom_RoomId",
                table: "EstateRoom",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstateRoom");

            migrationBuilder.AddColumn<Guid>(
                name: "EstateId",
                table: "Rooms",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Rooms",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ContractType",
                table: "Contracts",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_EstateId",
                table: "Rooms",
                column: "EstateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Estates_EstateId",
                table: "Rooms",
                column: "EstateId",
                principalTable: "Estates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
