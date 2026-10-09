using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OrderProcessing.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addedRolesConfigurationContainsDefaultRolesData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5f760592-2d05-4d66-b641-24484fc723f5", "f41f313c-cef8-432d-9aef-459d3734472c", "Admin", "ADMIN" },
                    { "a9c0c288-71a5-4916-9e37-d016f2a6728a", "a2dbbf7a-d81d-4bb4-81d7-4b4e45b8be84", "Customer", "CUSTOMER" },
                    { "db6e3551-f38a-45b5-bf7c-841a4cf6fe3f", "de4f68eb-3d5c-4edd-8b5c-a7af33fe14f0", "Vendor", "VENDOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5f760592-2d05-4d66-b641-24484fc723f5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a9c0c288-71a5-4916-9e37-d016f2a6728a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "db6e3551-f38a-45b5-bf7c-841a4cf6fe3f");
        }
    }
}
