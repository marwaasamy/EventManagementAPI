using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EventManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seedingRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "49750c33-447c-4d6c-9abd-30718d9ba67d", "f16ddde6-97b0-453c-a3a4-58fa5e0fb00a", "Admin", "ADMIN" },
                    { "a102a538-5768-456d-991d-c776361a9415", "a41176ab-3877-45f3-a75b-f1619b82f073", "User", "USER" },
                    { "c34d1d0b-c717-4b32-8157-da17a0b42a73", "288c5908-d12d-4816-9e79-410df6a1c09f", "Organizer", "ORGANIZER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "49750c33-447c-4d6c-9abd-30718d9ba67d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a102a538-5768-456d-991d-c776361a9415");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c34d1d0b-c717-4b32-8157-da17a0b42a73");
        }
    }
}
