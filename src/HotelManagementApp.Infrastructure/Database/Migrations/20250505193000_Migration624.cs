using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelManagementApp.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Migration624 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "DiscountCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    DiscountAmount = table.Column<int>(type: "INTEGER", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelDiscounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HotelId = table.Column<int>(type: "INTEGER", nullable: false),
                    From = table.Column<DateTime>(type: "TEXT", nullable: false),
                    To = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DiscountPercent = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelDiscounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelDiscounts_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HotelReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: false),
                    Review = table.Column<string>(type: "TEXT", nullable: false),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HotelId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelReviews_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoyaltyPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GuestId = table.Column<string>(type: "TEXT", nullable: false),
                    Points = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoyaltyPoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoyaltyPointsHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Points = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoyaltyPointsHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoyaltyRewards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RewardName = table.Column<string>(type: "TEXT", nullable: false),
                    PointsRequired = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoyaltyRewards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParkingDiscounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParkingId = table.Column<int>(type: "INTEGER", nullable: false),
                    From = table.Column<DateTime>(type: "TEXT", nullable: false),
                    To = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DiscountPercent = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingDiscounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParkingDiscounts_HotelParkings_ParkingId",
                        column: x => x.ParkingId,
                        principalTable: "HotelParkings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomDiscounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoomId = table.Column<int>(type: "INTEGER", nullable: false),
                    From = table.Column<DateTime>(type: "TEXT", nullable: false),
                    To = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DiscountPercent = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomDiscounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomDiscounts_HotelRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "HotelRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceDiscounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServiceId = table.Column<int>(type: "INTEGER", nullable: false),
                    From = table.Column<DateTime>(type: "TEXT", nullable: false),
                    To = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DiscountPercent = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceDiscounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceDiscounts_HotelServices_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "HotelServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CancelledOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CancelledOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CancelledOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompletedOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletedOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConfirmedOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfirmedOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfirmedOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: false),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PaymentMethod = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PendingOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PendingOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    From = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    To = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    RoomId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_HotelRooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "HotelRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "OrderStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "PENDING" },
                    { 2, "PAID" },
                    { 3, "CANCELLED" }
                });

            migrationBuilder.InsertData(
                table: "PaymentMethods",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "CASH" },
                    { 2, "CREDITCARD" },
                    { 3, "LOYALTYPOINTS" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_HotelServices_ReservationId",
                table: "HotelServices",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelParkings_ReservationId",
                table: "HotelParkings",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_CancelledOrders_OrderId",
                table: "CancelledOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedOrders_OrderId",
                table: "CompletedOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmedOrders_OrderId",
                table: "ConfirmedOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelDiscounts_HotelId",
                table: "HotelDiscounts",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelReviews_HotelId",
                table: "HotelReviews",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParkingDiscounts_ParkingId",
                table: "ParkingDiscounts",
                column: "ParkingId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PendingOrders_OrderId",
                table: "PendingOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_OrderId",
                table: "Reservations",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_RoomId",
                table: "Reservations",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomDiscounts_RoomId",
                table: "RoomDiscounts",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceDiscounts_ServiceId",
                table: "ServiceDiscounts",
                column: "ServiceId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelParkings_Reservations_ReservationId",
                table: "HotelParkings");

            migrationBuilder.DropForeignKey(
                name: "FK_HotelServices_Reservations_ReservationId",
                table: "HotelServices");

            migrationBuilder.DropTable(
                name: "CancelledOrders");

            migrationBuilder.DropTable(
                name: "CompletedOrders");

            migrationBuilder.DropTable(
                name: "ConfirmedOrders");

            migrationBuilder.DropTable(
                name: "DiscountCodes");

            migrationBuilder.DropTable(
                name: "HotelDiscounts");

            migrationBuilder.DropTable(
                name: "HotelReviews");

            migrationBuilder.DropTable(
                name: "LoyaltyPoints");

            migrationBuilder.DropTable(
                name: "LoyaltyPointsHistory");

            migrationBuilder.DropTable(
                name: "LoyaltyRewards");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "OrderStatuses");

            migrationBuilder.DropTable(
                name: "ParkingDiscounts");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PendingOrders");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "RoomDiscounts");

            migrationBuilder.DropTable(
                name: "ServiceDiscounts");

            migrationBuilder.DropTable(
                name: "Orders");

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
        }
    }
}
