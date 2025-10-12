using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel_chain.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReservaModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Reservas",
                type: "varchar(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuestEmail",
                table: "Reservas",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuestFirstName",
                table: "Reservas",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuestLastName",
                table: "Reservas",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuestPhone",
                table: "Reservas",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GuestsAdults",
                table: "Reservas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GuestsChildren",
                table: "Reservas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HotelId",
                table: "Reservas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Reservas",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RoomRate",
                table: "Reservas",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Subtotal",
                table: "Reservas",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Taxes",
                table: "Reservas",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_HotelId",
                table: "Reservas",
                column: "HotelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Hoteles_HotelId",
                table: "Reservas",
                column: "HotelId",
                principalTable: "Hoteles",
                principalColumn: "hotel_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Hoteles_HotelId",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_Reservas_HotelId",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "GuestEmail",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "GuestFirstName",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "GuestLastName",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "GuestPhone",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "GuestsAdults",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "GuestsChildren",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "RoomRate",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "Subtotal",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "Taxes",
                table: "Reservas");
        }
    }
}
