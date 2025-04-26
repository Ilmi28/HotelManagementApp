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
            migrationBuilder.DropForeignKey(
                name: "FK_HotelImage_Hotels_HotelId",
                table: "HotelImage");

            migrationBuilder.DropForeignKey(
                name: "FK_HotelParking_Hotels_HotelId",
                table: "HotelParking");

            migrationBuilder.DropForeignKey(
                name: "FK_HotelService_Hotels_HotelId",
                table: "HotelService");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomImage_Rooms_RoomId",
                table: "RoomImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomImage",
                table: "RoomImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HotelService",
                table: "HotelService");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HotelParking",
                table: "HotelParking");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HotelImage",
                table: "HotelImage");

            migrationBuilder.RenameTable(
                name: "RoomImage",
                newName: "RoomImages");

            migrationBuilder.RenameTable(
                name: "HotelService",
                newName: "HotelServices");

            migrationBuilder.RenameTable(
                name: "HotelParking",
                newName: "HotelParkings");

            migrationBuilder.RenameTable(
                name: "HotelImage",
                newName: "HotelImages");

            migrationBuilder.RenameIndex(
                name: "IX_RoomImage_RoomId",
                table: "RoomImages",
                newName: "IX_RoomImages_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_HotelService_HotelId",
                table: "HotelServices",
                newName: "IX_HotelServices_HotelId");

            migrationBuilder.RenameIndex(
                name: "IX_HotelParking_HotelId",
                table: "HotelParkings",
                newName: "IX_HotelParkings_HotelId");

            migrationBuilder.RenameIndex(
                name: "IX_HotelImage_HotelId",
                table: "HotelImages",
                newName: "IX_HotelImages_HotelId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "HotelImages",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomImages",
                table: "RoomImages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HotelServices",
                table: "HotelServices",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HotelParkings",
                table: "HotelParkings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HotelImages",
                table: "HotelImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelImages_Hotels_HotelId",
                table: "HotelImages",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HotelParkings_Hotels_HotelId",
                table: "HotelParkings",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HotelServices_Hotels_HotelId",
                table: "HotelServices",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomImages_Rooms_RoomId",
                table: "RoomImages",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelImages_Hotels_HotelId",
                table: "HotelImages");

            migrationBuilder.DropForeignKey(
                name: "FK_HotelParkings_Hotels_HotelId",
                table: "HotelParkings");

            migrationBuilder.DropForeignKey(
                name: "FK_HotelServices_Hotels_HotelId",
                table: "HotelServices");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomImages_Rooms_RoomId",
                table: "RoomImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomImages",
                table: "RoomImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HotelServices",
                table: "HotelServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HotelParkings",
                table: "HotelParkings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HotelImages",
                table: "HotelImages");

            migrationBuilder.RenameTable(
                name: "RoomImages",
                newName: "RoomImage");

            migrationBuilder.RenameTable(
                name: "HotelServices",
                newName: "HotelService");

            migrationBuilder.RenameTable(
                name: "HotelParkings",
                newName: "HotelParking");

            migrationBuilder.RenameTable(
                name: "HotelImages",
                newName: "HotelImage");

            migrationBuilder.RenameIndex(
                name: "IX_RoomImages_RoomId",
                table: "RoomImage",
                newName: "IX_RoomImage_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_HotelServices_HotelId",
                table: "HotelService",
                newName: "IX_HotelService_HotelId");

            migrationBuilder.RenameIndex(
                name: "IX_HotelParkings_HotelId",
                table: "HotelParking",
                newName: "IX_HotelParking_HotelId");

            migrationBuilder.RenameIndex(
                name: "IX_HotelImages_HotelId",
                table: "HotelImage",
                newName: "IX_HotelImage_HotelId");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "HotelImage",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomImage",
                table: "RoomImage",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HotelService",
                table: "HotelService",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HotelParking",
                table: "HotelParking",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HotelImage",
                table: "HotelImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelImage_Hotels_HotelId",
                table: "HotelImage",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HotelParking_Hotels_HotelId",
                table: "HotelParking",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HotelService_Hotels_HotelId",
                table: "HotelService",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomImage_Rooms_RoomId",
                table: "RoomImage",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
