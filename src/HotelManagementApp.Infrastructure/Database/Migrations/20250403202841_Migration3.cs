using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelManagementApp.Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class Migration3 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Admins");

        migrationBuilder.DropTable(
            name: "Clients");

        migrationBuilder.DropTable(
            name: "Managers");

        migrationBuilder.DropTable(
            name: "Workers");

        migrationBuilder.AlterColumn<int>(
            name: "Operation",
            table: "UserLogs",
            type: "INTEGER",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "TEXT");

        migrationBuilder.CreateTable(
            name: "Operation",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Operation", x => x.Id);
            });

        migrationBuilder.InsertData(
            table: "Operation",
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
            name: "Operation");

        migrationBuilder.AlterColumn<string>(
            name: "Operation",
            table: "UserLogs",
            type: "TEXT",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "INTEGER");

        migrationBuilder.CreateTable(
            name: "Admins",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                UserId = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Admins", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Clients",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                UserId = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Clients", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Managers",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                UserId = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Managers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Workers",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                UserId = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Workers", x => x.Id);
            });

        migrationBuilder.InsertData(
            table: "Admins",
            columns: new[] { "Id", "UserId" },
            values: new object[] { 1, "e3aff133-baa0-4931-a0af-0f67911cd6c3" });
    }
}
