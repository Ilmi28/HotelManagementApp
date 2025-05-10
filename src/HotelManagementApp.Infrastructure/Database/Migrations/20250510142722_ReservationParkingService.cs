using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManagementApp.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class ReservationParkingService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelParkings_Reservations_ReservationId",
                table: "HotelParkings");

            migrationBuilder.DropForeignKey(
                name: "FK_HotelServices_Reservations_ReservationId",
                table: "HotelServices");

            migrationBuilder.DropIndex(
                name: "IX_HotelServices_ReservationId",
                table: "HotelServices");

            migrationBuilder.DropIndex(
                name: "IX_HotelParkings_ReservationId",
                table: "HotelParkings");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "HotelServices");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "HotelParkings");

            migrationBuilder.CreateTable(
                name: "ReservationParkings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReservationId = table.Column<int>(type: "INTEGER", nullable: false),
                    HotelParkingId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationParkings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationParkings_HotelParkings_HotelParkingId",
                        column: x => x.HotelParkingId,
                        principalTable: "HotelParkings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationParkings_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservationServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReservationId = table.Column<int>(type: "INTEGER", nullable: false),
                    HotelServiceId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationServices_HotelServices_HotelServiceId",
                        column: x => x.HotelServiceId,
                        principalTable: "HotelServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationServices_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "CONFIRMED");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationParkings_HotelParkingId",
                table: "ReservationParkings",
                column: "HotelParkingId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationParkings_ReservationId",
                table: "ReservationParkings",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationServices_HotelServiceId",
                table: "ReservationServices",
                column: "HotelServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationServices_ReservationId",
                table: "ReservationServices",
                column: "ReservationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationParkings");

            migrationBuilder.DropTable(
                name: "ReservationServices");

            migrationBuilder.AddColumn<int>(
                name: "ReservationId",
                table: "HotelServices",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReservationId",
                table: "HotelParkings",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "OrderStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "PAID");

            migrationBuilder.CreateIndex(
                name: "IX_HotelServices_ReservationId",
                table: "HotelServices",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelParkings_ReservationId",
                table: "HotelParkings",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelParkings_Reservations_ReservationId",
                table: "HotelParkings",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelServices_Reservations_ReservationId",
                table: "HotelServices",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id");
        }
    }
}
