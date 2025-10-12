using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel_chain.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHotelModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Calificacion",
                table: "Hoteles",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CantidadResenas",
                table: "Hoteles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckInTime",
                table: "Hoteles",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckOutTime",
                table: "Hoteles",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactoEmail",
                table: "Hoteles",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Hoteles",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Hoteles",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FumarPermitido",
                table: "Hoteles",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Latitud",
                table: "Hoteles",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitud",
                table: "Hoteles",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MascotasPermitidas",
                table: "Hoteles",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Moneda",
                table: "Hoteles",
                type: "varchar(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pais",
                table: "Hoteles",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PoliticaCancelacion",
                table: "Hoteles",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecioMax",
                table: "Hoteles",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecioMin",
                table: "Hoteles",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Hoteles",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Calificacion",
                table: "Hoteles");

            migrationBuilder.DropColumn(
                name: "CantidadResenas",
                table: "Hoteles");

            migrationBuilder.DropColumn(
                name: "CheckInTime",
                table: "Hoteles");

            migrationBuilder.DropColumn(
                name: "CheckOutTime",
                table: "Hoteles");

            migrationBuilder.DropColumn(
                name: "ContactoEmail",
                table: "Hoteles");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Hoteles");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Hoteles");

            migrationBuilder.DropColumn(
                name: "FumarPermitido",
                table: "Hoteles");

            migrationBuilder.DropColumn(
                name: "Latitud",
                table: "Hoteles");

            migrationBuilder.DropColumn(
                name: "Longitud",
                table: "Hoteles");

            migrationBuilder.DropColumn(
                name: "MascotasPermitidas",
                table: "Hoteles");

            migrationBuilder.DropColumn(
                name: "Moneda",
                table: "Hoteles");

            migrationBuilder.DropColumn(
                name: "Pais",
                table: "Hoteles");

            migrationBuilder.DropColumn(
                name: "PoliticaCancelacion",
                table: "Hoteles");

            migrationBuilder.DropColumn(
                name: "PrecioMax",
                table: "Hoteles");

            migrationBuilder.DropColumn(
                name: "PrecioMin",
                table: "Hoteles");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Hoteles");
        }
    }
}
