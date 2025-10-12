using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel_chain.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUsuarioModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "telefono",
                table: "Usuarios",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "Usuarios",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "documento",
                table: "Usuarios",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualizadoEn",
                table: "Usuarios",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Usuarios",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoPostal",
                table: "Usuarios",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactoEmergenciaNombre",
                table: "Usuarios",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactoEmergenciaRelacion",
                table: "Usuarios",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactoEmergenciaTelefono",
                table: "Usuarios",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Departamento",
                table: "Usuarios",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DireccionCalle",
                table: "Usuarios",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DireccionCiudad",
                table: "Usuarios",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DireccionEstado",
                table: "Usuarios",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DireccionPais",
                table: "Usuarios",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaIngreso",
                table: "Usuarios",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HotelAsignado",
                table: "Usuarios",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notas",
                table: "Usuarios",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Salario",
                table: "Usuarios",
                type: "decimal(10,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualizadoEn",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "CodigoPostal",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "ContactoEmergenciaNombre",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "ContactoEmergenciaRelacion",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "ContactoEmergenciaTelefono",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Departamento",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "DireccionCalle",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "DireccionCiudad",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "DireccionEstado",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "DireccionPais",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "FechaIngreso",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "HotelAsignado",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Notas",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Salario",
                table: "Usuarios");

            migrationBuilder.AlterColumn<string>(
                name: "telefono",
                table: "Usuarios",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "Usuarios",
                type: "varchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "documento",
                table: "Usuarios",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
