using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garage.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeVehicleIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vehicles_Plate",
                schema: "Garage",
                table: "Vehicles");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_Plate_UserId",
                schema: "Garage",
                table: "Vehicles",
                columns: new[] { "Plate", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vehicles_Plate_UserId",
                schema: "Garage",
                table: "Vehicles");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_Plate",
                schema: "Garage",
                table: "Vehicles",
                column: "Plate",
                unique: true);
        }
    }
}
