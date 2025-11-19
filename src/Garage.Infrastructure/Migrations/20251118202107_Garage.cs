using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Garage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GarageId",
                schema: "Garage",
                table: "Vehicles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Garages",
                schema: "Garage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UserId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Garages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_GarageId",
                schema: "Garage",
                table: "Vehicles",
                column: "GarageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Garages_GarageId",
                schema: "Garage",
                table: "Vehicles",
                column: "GarageId",
                principalSchema: "Garage",
                principalTable: "Garages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Garages_GarageId",
                schema: "Garage",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "Garages",
                schema: "Garage");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_GarageId",
                schema: "Garage",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "GarageId",
                schema: "Garage",
                table: "Vehicles");
        }
    }
}
