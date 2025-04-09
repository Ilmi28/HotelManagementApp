using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManagementApp.Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class Migration4 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_Tokens",
            table: "Tokens");

        migrationBuilder.RenameTable(
            name: "Tokens",
            newName: "RefreshTokens");

        migrationBuilder.AddPrimaryKey(
            name: "PK_RefreshTokens",
            table: "RefreshTokens",
            column: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_RefreshTokens",
            table: "RefreshTokens");

        migrationBuilder.RenameTable(
            name: "RefreshTokens",
            newName: "Tokens");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Tokens",
            table: "Tokens",
            column: "Id");
    }
}
