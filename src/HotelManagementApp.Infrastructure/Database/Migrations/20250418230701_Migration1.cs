using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelManagementApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountLogs");

            migrationBuilder.DeleteData(
                table: "AccountHistory",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AccountHistory",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AccountHistory",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AccountHistory",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AccountHistory",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AccountHistory",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AccountHistory",
                newName: "UserId");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "AccountHistory",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Operation",
                table: "AccountHistory",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AccountOperations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountOperations", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AccountOperations",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "REGISTER" },
                    { 2, "LOGIN" },
                    { 3, "CREATE" },
                    { 4, "UPDATE" },
                    { 5, "DELETE" },
                    { 6, "PASSWORD CHANGE" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountOperations");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "AccountHistory");

            migrationBuilder.DropColumn(
                name: "Operation",
                table: "AccountHistory");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AccountHistory",
                newName: "Name");

            migrationBuilder.CreateTable(
                name: "AccountLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Operation = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountLogs", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AccountHistory",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "REGISTER" },
                    { 2, "LOGIN" },
                    { 3, "CREATE" },
                    { 4, "UPDATE" },
                    { 5, "DELETE" },
                    { 6, "PASSWORD CHANGE" }
                });
        }
    }
}
