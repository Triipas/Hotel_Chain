using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel_chain.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHabitacionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "tipo",
                table: "Habitaciones",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AddColumn<int>(
                name: "CantidadCamas",
                table: "Habitaciones",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CapacidadAdultos",
                table: "Habitaciones",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CapacidadNinos",
                table: "Habitaciones",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Habitaciones",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "HabitacionesDisponibles",
                table: "Habitaciones",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Moneda",
                table: "Habitaciones",
                type: "varchar(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Habitaciones",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecioBase",
                table: "Habitaciones",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecioImpuestos",
                table: "Habitaciones",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecioTotal",
                table: "Habitaciones",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TamanoM2",
                table: "Habitaciones",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoCama",
                table: "Habitaciones",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Habitaciones",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CantidadCamas",
                table: "Habitaciones");

            migrationBuilder.DropColumn(
                name: "CapacidadAdultos",
                table: "Habitaciones");

            migrationBuilder.DropColumn(
                name: "CapacidadNinos",
                table: "Habitaciones");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Habitaciones");

            migrationBuilder.DropColumn(
                name: "HabitacionesDisponibles",
                table: "Habitaciones");

            migrationBuilder.DropColumn(
                name: "Moneda",
                table: "Habitaciones");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Habitaciones");

            migrationBuilder.DropColumn(
                name: "PrecioBase",
                table: "Habitaciones");

            migrationBuilder.DropColumn(
                name: "PrecioImpuestos",
                table: "Habitaciones");

            migrationBuilder.DropColumn(
                name: "PrecioTotal",
                table: "Habitaciones");

            migrationBuilder.DropColumn(
                name: "TamanoM2",
                table: "Habitaciones");

            migrationBuilder.DropColumn(
                name: "TipoCama",
                table: "Habitaciones");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Habitaciones");

            migrationBuilder.AlterColumn<string>(
                name: "tipo",
                table: "Habitaciones",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);
        }
    }
}
