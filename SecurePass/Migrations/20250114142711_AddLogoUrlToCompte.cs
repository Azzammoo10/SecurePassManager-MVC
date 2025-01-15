using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WatchlistV2.Migrations
{
    /// <inheritdoc />
    public partial class AddLogoUrlToCompte : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "05ca9adc-cddd-4516-bff9-3fd2a2016eb6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "93164718-b090-4eb9-8b0c-5618591ddcc4");

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "Comptes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1e748715-6eb9-4b39-a162-471a2c74db1f", null, "User", "USER" },
                    { "cc82f667-e19b-438b-98b3-2604d3438908", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e748715-6eb9-4b39-a162-471a2c74db1f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc82f667-e19b-438b-98b3-2604d3438908");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "Comptes");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "05ca9adc-cddd-4516-bff9-3fd2a2016eb6", null, "Admin", "ADMIN" },
                    { "93164718-b090-4eb9-8b0c-5618591ddcc4", null, "User", "USER" }
                });
        }
    }
}
