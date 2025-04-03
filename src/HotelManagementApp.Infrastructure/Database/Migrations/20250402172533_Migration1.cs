using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManagementApp.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Client", "CLIENT" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "User", "USER" });
        }
    }
}
