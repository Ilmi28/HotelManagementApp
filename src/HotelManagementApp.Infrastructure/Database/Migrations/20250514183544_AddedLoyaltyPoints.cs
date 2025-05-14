using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManagementApp.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedLoyaltyPoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoyaltyRewardUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoyaltyRewardId = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoyaltyRewardUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoyaltyRewardUsers_LoyaltyRewards_LoyaltyRewardId",
                        column: x => x.LoyaltyRewardId,
                        principalTable: "LoyaltyRewards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoyaltyRewardUsers_LoyaltyRewardId",
                table: "LoyaltyRewardUsers",
                column: "LoyaltyRewardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoyaltyRewardUsers");
        }
    }
}
